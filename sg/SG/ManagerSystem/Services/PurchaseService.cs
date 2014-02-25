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

        public void processAutomaticPurchase(int offer_id)
        {
            OfferEntity offer = offerService.getOffer(offer_id);
            ExpPedido e_order_confirmation = new ExpPedido();
            e_order_confirmation.lineas = new List<ExpPedido.Line>();

            foreach (var offer_line in offer.lines)
            {
                OrderLineEntity order_line = offer_line.order_line;
                if (order_line.flag == OrderLineFlag.FIRST)
                {
                    this.updateOfferAndOrderLines(offer_line, order_line);
                    this.addExpPedidoLine(e_order_confirmation, offer_line);
                }
            }

            if (e_order_confirmation.lineas.Count > 0)
            {
                e_order_confirmation.oferta_id = offer.id;
                this.notifyOrder(e_order_confirmation, offer.junkyard.current_token);
            }
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
        }

        private void addExpPedidoLine(ExpPedido e_order_confirmation, OfferLineEntity offer_line)
        {
            e_order_confirmation.lineas.Add(new ExpPedido.Line
            {
                linea_oferta_id = offer_line.id,
                quantity = offer_line.selected_ammount
            });
        }

        public void selectOffer(ExpPedido e_selected_offers)
        {
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

            // Change the id of the offer to the one in the Junkyard
            OfferEntity offer = offerService.getOffer(e_selected_offers.oferta_id);
            e_selected_offers.oferta_id = offer.corresponding_id;

            this.notifyOrder(e_selected_offers, offer.junkyard.current_token);
        }

        private void validateOfferLine(OfferLineEntity offer_line, int offer_id)
        {
            GarageEntity current_garage = garageService.getCurrentGarage();
            if (offer_line.order_line.order.garage != current_garage)
                throw new ArgumentException(String.Format(
                    "The OrderLine with id {0} does not belong to the Garage with token {1}",
                    offer_line.order_line_id,
                    authorizationService.getCurrentGarageToken()));

            if (offer_line.offer_id != offer_id)
                throw new ArgumentException(String.Format(
                    "The OfferLine with id {0} does not belong to the Offer {1}",
                    offer_line.order_line_id,
                    offer_id));
        }

        private void notifyOrder(ExpPedido e_order_confirmation, string junkyard_token)
        {
            AMQPedidoMessage msg = new AMQPedidoMessage
            {
                pedido = e_order_confirmation,
                desguace_id = junkyard_token
            };
            amqService.publishOrderConfirmation(msg);
        }
    }
}