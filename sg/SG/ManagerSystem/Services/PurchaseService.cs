using ManagerSystem.DataAccess;
using ManagerSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem.Services
{
    public class PurchaseService : BaseService
    {
        public PurchaseService(UnitOfWork uow = null) : base(uow) { }

        public void processOnOfferPurchase(int offer_id)
        {
            OfferEntity offer = offerService.getOffer(offer_id);
            ExpPedido e_order_confirmation = new ExpPedido();

            foreach (var offer_line in offer.lines)
            {
                OrderLineEntity order_line = offer_line.order_line;
                if (order_line.flag == OrderLineFlag.FIRST)
                {
                    this.updateOfferAndOrderLines(offer_line, order_line);
                    this.addExpPedidoLine(e_order_confirmation, offer_line);
                }
            }
            unitOfWork.Save();

            if (e_order_confirmation.lineas.Count > 0)
            {
                e_order_confirmation.oferta_id = offer.id;
                amqService.publishOrderConfirmation(e_order_confirmation, offer.junkyard.current_token);
            }
        }

        public void processOnDeadlinePurchases(int order_id)
        {
            OrderEntity order = orderService.getOrder(order_id);
            List<OfferLineEntity> selected_offers = new List<OfferLineEntity>();

            foreach (var order_line in order.lines)
            {
                List<OfferLineEntity> ordered_offers = orderService.getOrderedOfferLines(order_line);
                selected_offers.AddRange(this.selectOfferLines(order_line, ordered_offers));
            }
            unitOfWork.Save();

            this.notifyMultipleOffers(selected_offers);
        }

        private void notifyMultipleOffers(List<OfferLineEntity> selected_offers)
        {
            var grouped_list = selected_offers.GroupBy(o_l => o_l.offer_id);
            foreach (var group in grouped_list)
            {
                List<OfferLineEntity> offer_lines = group.ToList();
                OfferEntity offer = offer_lines[0].offer;

                ExpPedido e_order_confirmation = new ExpPedido
                {
                    oferta_id = offer.corresponding_id,
                    lineas = (from offer_line in offer_lines
                              select new ExpPedido.Line
                              {
                                  linea_oferta_id = offer_line.id,
                                  quantity = offer_line.quantity
                              }).ToList()
                };
                amqService.publishOrderConfirmation(e_order_confirmation, offer.junkyard.current_token);
            }
        }

        private List<OfferLineEntity> selectOfferLines(OrderLineEntity order_line, List<OfferLineEntity> ordered_offers)
        {
            List<OfferLineEntity> selected_offers = new List<OfferLineEntity>();

            foreach (var offer_line in ordered_offers)
            {
                if (order_line.status == OrderLineStatus.COMPLETE)
                    break;

                this.selectFromOrderLine(order_line, offer_line);
                selected_offers.Add(offer_line);
            }

            return selected_offers;
        }

        private void selectFromOrderLine(OrderLineEntity order_line, OfferLineEntity offer_line)
        {
            int quantity_remaining = order_line.quantity - order_line.selected_ammount;
            offer_line.status = OfferLineStatus.SELECTED;

            if (offer_line.quantity < quantity_remaining)
            {
                offer_line.selected_ammount = offer_line.quantity;
                order_line.status = OrderLineStatus.INCOMPLETE;
            }
            else
            {
                offer_line.selected_ammount = quantity_remaining;
                order_line.status = OrderLineStatus.COMPLETE;
            }
            unitOfWork.OfferLineRepository.Update(offer_line);
            unitOfWork.OrderLineRepository.Update(order_line);
        }

        public void selectOffer(ExpPedido e_selected_offers)
        {
            OfferEntity offer = offerService.getOffer(e_selected_offers.oferta_id);

            authService.forbidGarageAccess(offer.order.garage_id);

            // Change the id of the offer to the one in the Junkyard
            e_selected_offers.oferta_id = offer.corresponding_id;

            foreach (var e_selected_line in e_selected_offers.lineas)
            {
                OfferLineEntity offer_line = offerService.getOfferLine(e_selected_line.linea_oferta_id);
                this.validateOfferLine(offer_line, e_selected_offers.oferta_id);

                offerService.selectOfferLine(offer_line.id, e_selected_line.quantity);

                OrderLineEntity order_line = orderService.getOrderLine(offer_line.order_line_id);
                orderService.updateOrderLineStatus(order_line);
        
                // Change the id of the line to the one in the Junkyard
                e_selected_line.linea_oferta_id = offer_line.corresponding_id;
            }
            unitOfWork.Save();

            amqService.publishOrderConfirmation(e_selected_offers, offer.junkyard.current_token);
        }

        private void validateOfferLine(OfferLineEntity offer_line, int offer_id)
        {
            GarageEntity current_garage = garageService.getCurrentGarage();
            if (offer_line.order_line.order.garage != current_garage)
                throw new ArgumentException(String.Format(
                    "The OrderLine with id {0} does not belong to the Garage with token {1}",
                    offer_line.order_line_id,
                    authService.getCurrentGarageToken()));

            if (offer_line.offer_id != offer_id)
                throw new ArgumentException(String.Format(
                    "The OfferLine with id {0} does not belong to the Offer {1}",
                    offer_line.order_line_id,
                    offer_id));
        }

        private void updateOfferAndOrderLines(OfferLineEntity offer_line, OrderLineEntity order_line)
        {
            offer_line.status = OfferLineStatus.SELECTED;
            if (order_line.quantity > offer_line.quantity)
            {
                order_line.status = OrderLineStatus.INCOMPLETE;
                offer_line.selected_ammount = offer_line.quantity;
            }
            else
            {
                order_line.status = OrderLineStatus.COMPLETE;
                offer_line.selected_ammount = order_line.quantity;
            }
            unitOfWork.OrderLineRepository.Update(order_line);
            unitOfWork.OfferLineRepository.Update(offer_line);
        }

        private void addExpPedidoLine(ExpPedido e_order_confirmation, OfferLineEntity offer_line)
        {
            e_order_confirmation.lineas.Add(new ExpPedido.Line
            {
                linea_oferta_id = offer_line.id,
                quantity = offer_line.selected_ammount
            });
        }
    }
}