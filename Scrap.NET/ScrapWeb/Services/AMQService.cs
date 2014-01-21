using ScrapWeb.AMQ;
using ScrapWeb;
using ScrapWeb.Entities;
using ScrapWeb.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using ScrapWeb.Webservices;

namespace ScrapWeb.Services
{
    public class AMQService
    {
        private static readonly string originSolicitudes = "Solicitudes";
        private static readonly string originPedidos = "Pedidos";
        private static readonly string destination = "Ofertas";

        private TopicSubscriber topicSubscriberSolicitudes;
        private TopicSubscriber topicSubscriberPedidos;

        private static TopicPublisher _topicPublisher;
        private static TopicPublisher topicPublisher 
        {
            get 
            {
                if(_topicPublisher == null) 
                {
                    _topicPublisher = new TopicPublisher(AMQConfig.Session, AMQConfig.Connection, destination);
                }
                return _topicPublisher;
            }

            set
            {
                _topicPublisher = value;
            }
        }

        public void createTopicSubscribers() 
        {
            try
            {
                TokenService tokenService = new TokenService();
                TokenEntity token = tokenService.getValid();
                this.createTopicSubscribers(token);
            }
            catch (ServiceException ex)
            {
                // We dont have a valid token, we don't create a topic subscriber
                Trace.WriteLine("Not valid token found. Disabling topic subscriber...");
            }
        }

        public void createTopicSubscribers(TokenEntity validToken)
        {
            Trace.WriteLine("Valid token found! Enabling topic subscribers...");
            // Creating subscriber to solicitudes
            topicSubscriberSolicitudes = new TopicSubscriber(AMQConfig.Session, originSolicitudes);
            topicSubscriberSolicitudes.OnMessageReceived += topicSubscriberSolicitudes_OnMessageReceived;
            // Creating subscriber to pedidos
            topicSubscriberPedidos = new TopicSubscriber(AMQConfig.Session, originPedidos);
            topicSubscriberPedidos.OnMessageReceived += topicSubscriberPedidos_OnMessageReceived;
            topicSubscriberSolicitudes.Start(validToken.token + "@Solicitudes");
            topicSubscriberPedidos.Start(validToken.token + "@Pedidos");
        }

        public void destroyTopicSubscribers()
        {
            destroyTopicSubscriber(topicSubscriberPedidos);
            destroyTopicSubscriber(topicSubscriberSolicitudes);
        }

        private void destroyTopicSubscriber(TopicSubscriber topicSubscriber) 
        {
            if (topicSubscriber != null)
            {
                topicSubscriber.Dispose();
                topicSubscriber = null;
            }
        }

        void topicSubscriberSolicitudes_OnMessageReceived(string message)
        {
            AMQSolicitudMessage solicitudMessage = 
                (AMQSolicitudMessage)topicSubscriberSolicitudes.FromXML(message, (new AMQSolicitudMessage()).GetType());
            OrderEntity orderEntity = toOrder(solicitudMessage);
            Trace.WriteLine("Received order with remote id " + orderEntity.sgId);
            OrderService orderService = new OrderService();
            orderService.save(orderEntity);
        }

        void topicSubscriberPedidos_OnMessageReceived(string message)
        {
            TokenService tokenService = new TokenService();
            TokenEntity tokenEntity = tokenService.getValid();
            AMQPedidoMessage pedidoMessage =
                (AMQPedidoMessage)topicSubscriberPedidos.FromXML(message, (new AMQPedidoMessage()).GetType());
            if(pedidoMessage.desguace_id == tokenEntity.token)
            {
                OfferService offerService = new OfferService();
                Trace.WriteLine(
                    "Saving accepted offer for me with remote id " + pedidoMessage.pedido.oferta_id.ToString() + "...");
                offerService.update(toAcceptedOffers(pedidoMessage, offerService));
            }
            else 
            {
                Trace.WriteLine(
                    "Ignoring accepted offer for another junkyard with remote id " + pedidoMessage.pedido.oferta_id.ToString()  + "...");
            }
        }

        private List<OfferLineEntity> toAcceptedOffers(AMQPedidoMessage pedidoMessage, OfferService offerService) 
        {
            List<OfferLineEntity> offerlines = new List<OfferLineEntity>();
            foreach (var line in pedidoMessage.pedido.lineas)
            {
                // Get existing offerline with this sgId
                var offerline = offerService.getOfferLine(line.linea_oferta_id);
                offerline.acceptedOffer = new AcceptedOfferLineEntity
                {
                    quantity = line.quantity
                };
                offerlines.Add(offerline);
            }
            return offerlines;
        }

        private OrderEntity toOrder(AMQSolicitudMessage solicitudMessage)
        {
            List<OrderLineEntity> lines = new List<OrderLineEntity>();
            foreach(var line in solicitudMessage.solicitud.lineas) 
            {
                lines.Add(new OrderLineEntity
                {
                    description = line.description,
                    quantity = line.quantity,
                    sgId = line.id.ToString()
                });
            }

            return new OrderEntity
            {
                rawLines = lines,
                sgId = solicitudMessage.solicitud.id.ToString(),
                deadline = solicitudMessage.solicitud.deadline
            };
        }

        public void sendOffer(OfferEntity offerEntity)
        {
            Trace.WriteLine("Sending new offer with remote id " + offerEntity.orderSgId + " ...");
            topicPublisher.SendMessage(toAMQOfertaMessage(offerEntity, AMQOfertaMessageCode.New));
        }

        public void updateOffer(OfferEntity offerEntity)
        {
            Trace.WriteLine("Updating offer with remote id " + offerEntity.orderSgId + " ...");
            topicPublisher.SendMessage(toAMQOfertaMessage(offerEntity, AMQOfertaMessageCode.Update));
        }

        public void deleteOffer(OfferEntity offerEntity)
        {
            Trace.WriteLine("Deleting offer with remote id " + offerEntity.orderSgId + " ...");
            topicPublisher.SendMessage(toAMQOfertaMessage(offerEntity, AMQOfertaMessageCode.Delete));
        }

        private AMQOfertaMessage toAMQOfertaMessage(OfferEntity offerEntity, AMQOfertaMessageCode status)
        {
            // Get token (if not valid, we finish here)
            TokenService tokenService = new TokenService();
            var token = tokenService.getValid();

            // Get lines
            var lineas = new List<ExpOfertaLine>();
            foreach (var line in offerEntity.rawLines) 
            {
                lineas.Add(new ExpOfertaLine 
                { 
                     linea_solicitud_id = int.Parse(line.orderLine.sgId),
                     id_en_desguace = line.id,
                     notes = line.notes,
                     price = line.price,
                     quantity = line.quantity
                });
            }

            // Get offer
            var oferta = new ExpOferta
            {
                solicitud_id = int.Parse(offerEntity.orderSgId),
                id_en_desguace = offerEntity.id,
                lineas = lineas.ToArray()
            };

            // Get message
            return new AMQOfertaMessage
            {
                desguace_id = token.token,
                code = status,
                oferta = oferta
            };
        }
    }
}