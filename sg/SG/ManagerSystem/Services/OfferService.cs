using ManagerSystem.DataAccess;
using ManagerSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem.Services
{
    public class OfferService : BaseService
    {
        public OfferService(UnitOfWork uow = null) : base(uow) { }

        public OfferEntity getOffer(int offer_id)
        {
            OfferEntity offer = unitOfWork.OfferRepository.GetByID(offer_id, "lines");

            if (offer == null)
                throw new ArgumentNullException();

            if (offer.order.garage != garageService.getCurrentGarage())
                throw new ArgumentException();

            return offer;
        }

        public List<OfferEntity> getOffers(int order_id)
        {
            OrderEntity order = orderService.getOrder(order_id);

            List<OfferEntity> offers = new List<OfferEntity>();
            foreach (var offer in order.getOffers())
            {
                offers.Add(this.getOffer(offer.id));
            }

            return offers;
        }

        public void addOffer(ExpOferta e_offer)
        {
            OfferEntity offer = new OfferEntity();

            this.copyFromExposed(offer, e_offer);

            unitOfWork.OfferRepository.Insert(offer);
            unitOfWork.Save();
        }

        public void putOffer(ExpOferta e_offer)
        {
            OfferEntity offer = unitOfWork.OfferRepository.GetByID(e_offer.id);

            if (offer == null)
                throw new ArgumentNullException();

            if (offer.junkyard != junkyardService.getCurrentJunkyard())
                throw new ArgumentException();

            this.copyFromExposed(offer, e_offer);

            unitOfWork.OfferRepository.Update(offer);
            unitOfWork.Save();
        }

        public void deleteOffer(int offer_id)
        {
            OfferEntity offer = unitOfWork.OfferRepository.GetByID(offer_id);

            if (offer == null)
                throw new ArgumentNullException();

            if (offer.junkyard != junkyardService.getCurrentJunkyard())
                throw new ArgumentException();

            unitOfWork.OfferRepository.Delete(offer);
            unitOfWork.Save();
        }

        public OfferLineEntity getOfferLine(int offer_line_id)
        {
            OfferLineEntity line = unitOfWork.OfferLineRepository.GetByID(offer_line_id);

            if (line == null)
                throw new ArgumentException(String.Format("There is not an OfferLine with id {0}", offer_line_id));

            return line;
        }

        public void selectOfferLine(int offer_line_id, int ammount)
        {
            OfferLineEntity offer_line = this.getOfferLine(offer_line_id);

            if (offer_line.status == OfferLineStatus.SELECTED)
                throw new ArgumentException(String.Format(
                    "The OfferLine {0} is already complete",
                    offer_line_id));

            if (ammount <= 0)
                throw new ArgumentException(String.Format(
                    "Ammount {0} has to be greater than 0",
                    ammount));

            offer_line.selected_ammount += ammount;
            offer_line.status = OfferLineStatus.SELECTED;

            orderService.updateOrderLineStatus(offer_line.order_line_id);

            unitOfWork.OfferLineRepository.Update(offer_line);
        }

        public void selectOffer(ExpPedido e_selected_offers)
        {
            GarageEntity current_garage = garageService.getCurrentGarage();

            foreach (var e_selected_line in e_selected_offers.lineas)
            {
                OfferLineEntity offer_line = this.getOfferLine(e_selected_line.linea_oferta_id);

                if (offer_line.order_line.order.garage != current_garage)
                    throw new ArgumentException(String.Format(
                        "The OrderLine with id {0} does not belong to the Garage with token {1}",
                        offer_line.order_line_id,
                        authorizationService.getCurrentGarageToken()));

                if (offer_line.offer_id != e_selected_offers.oferta_id)
                    throw new ArgumentException(String.Format(
                        "The OfferLine with id {0} does not belong to the Offer {1}", 
                        e_selected_line.linea_oferta_id, 
                        e_selected_offers.oferta_id));

                this.selectOfferLine(offer_line.id, e_selected_line.quantity);

                // Change the id of the line to the one in the Junkyard
                e_selected_line.linea_oferta_id = offer_line.corresponding_id;
            }
            unitOfWork.Save();

            // Change the id of the offer to the one in the Junkyard
            OfferEntity offer = this.getOffer(e_selected_offers.oferta_id);
            e_selected_offers.oferta_id = offer.corresponding_id;

            AMQPedidoMessage msg = new AMQPedidoMessage
            {
                pedido = e_selected_offers,
                desguace_id = offer.junkyard.current_token
            };
            amqService.publishOrderConfirmation(msg);
        }

        public void copyFromExposed(OfferEntity offer, ExpOferta e_offer)
        {
            offer.corresponding_id = e_offer.id_en_desguace;
            offer.junkyard         = junkyardService.getCurrentJunkyard();
            offer.status           = OfferStatus.NEW;

            // Remove the old lines, if any
            foreach (var line in offer.lines) 
            {
                unitOfWork.OfferLineRepository.Delete(line.id);
            }
            offer.lines.Clear();

            // Add the new ones
            foreach (var e_line in e_offer.lineas)
            {
                OfferLineEntity line = new OfferLineEntity();
                this.copyLineFromExposed(line, e_line);
                line.offer = offer;
                unitOfWork.OfferLineRepository.Insert(line);
            }
        }

        public void copyLineFromExposed(OfferLineEntity line, ExpOferta.Line e_line)
        {
            line.corresponding_id = e_line.id_en_desguace;
            line.order_line = orderService.getOrderLine(e_line.linea_solicitud_id);
            line.price = e_line.price;
            line.quantity = e_line.quantity;
            line.notes = e_line.notes;
        }

        public ExpOferta toExposed(OfferEntity offer)
        {
            ExpOferta e_offer = new ExpOferta();
            e_offer.id = offer.id;
            e_offer.solicitud_id = offer.order.id;
            e_offer.lineas = new List<ExpOferta.Line>();
            foreach (var line in offer.lines)
            {
                e_offer.lineas.Add(this.toExposed(line));                
            }

            return e_offer;
        }

        public ExpOferta.Line toExposed(OfferLineEntity line)
        {
            ExpOferta.Line e_line = new ExpOferta.Line
            {
                id = line.id,
                id_en_desguace = line.corresponding_id,
                linea_solicitud_id = line.order_line_id,
                notes = line.notes,
                price = line.price,
                quantity = line.quantity,
                selected_ammount = line.selected_ammount
            };

            return e_line;
        }
    }
}