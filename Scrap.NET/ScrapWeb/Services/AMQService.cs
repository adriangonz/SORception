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
        private readonly string destination = "Solicitudes";
        private TopicSubscriber topicSubscriber;

        public TopicSubscriber createTopicSubscriber() 
        {
            try
            {
                TokenService tokenService = new TokenService();
                TokenEntity token = tokenService.getValid();
                return this.createTopicSubscriber(token);
            }
            catch (ServiceException ex)
            {
                // We dont have a valid token, we don't create a topic subscriber
                Trace.WriteLine("Not valid token found. Disabling topic subscriber...");
            }
            return null;
        }

        public TopicSubscriber createTopicSubscriber(TokenEntity validToken)
        {
            Trace.WriteLine("Valid token found! Enabling topic subscriber...");
            topicSubscriber = new TopicSubscriber(AMQConfig.Session, destination);
            topicSubscriber.OnMessageReceived += topicSubscriber_OnMessageReceived;
            topicSubscriber.Start(validToken.token);
            return topicSubscriber;
        }

        public void destroyTopicSubscriber()
        {
            if (topicSubscriber != null)
            {
                topicSubscriber.Dispose();
                topicSubscriber = null;
            }
        }

        void topicSubscriber_OnMessageReceived(string message)
        {
            AMQSolicitudMessage solicitudMessage = (AMQSolicitudMessage)topicSubscriber.FromXML(message, (new AMQSolicitudMessage()).GetType());
            OrderEntity orderEntity = toOrder(solicitudMessage);
            Trace.WriteLine("Received order with remote id " + orderEntity.sgId);
            OrderService orderService = new OrderService();
            orderService.save(orderEntity);
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
                lines = lines,
                sgId = solicitudMessage.solicitud.id.ToString(),
                deadline = solicitudMessage.solicitud.deadline
            };
        }
    }
}