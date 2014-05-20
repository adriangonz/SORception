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

        public string current_junkyard = "";

        public OfferEntity getOffer(int offer_id)
        {
            OfferEntity offer = unitOfWork.OfferRepository.GetByID(offer_id, "lines");
            if (offer == null)
                throw new ArgumentNullException();

            authService.restrictAccess(garage: offer.order.garage, junkyard: offer.junkyard);

            return offer;
        }

        public List<OfferEntity> getOffers(int order_id)
        {
            OrderEntity order = orderService.getOrder(order_id);

            authService.restrictAccess(garage: order.garage);

            List<OfferEntity> offers = new List<OfferEntity>();
            foreach (var offer in order.offers)
            {
                offers.Add(this.getOffer(offer.id));
            }

            return offers;
        }

        public void addOffer(ExpOferta e_offer)
        {
            if (this.offerIdExists(e_offer.id))
            {
                throw new ArgumentException("There is already an offer with id " + e_offer.id);
            }
            
            authService.setJunkyardToken(current_junkyard);
            try
            {
                OfferEntity offer = new OfferEntity();

                this.copyFromExposed(offer, e_offer);
                unitOfWork.OfferRepository.Insert(offer);
                unitOfWork.Save();

                purchaseService.processOnOfferPurchase(offer.order.id);
            }
            catch (Exception e)
            {
                logService.Info(e.ToString());
                throw;
            }
        }

        private bool offerIdExists(int offer_id)
        {
            return unitOfWork.OfferRepository.GetByID(offer_id) != null;
        }

        public void putOffer(ExpOferta e_offer)
        {
            OfferEntity offer = unitOfWork.OfferRepository.GetByID(e_offer.id);

            if (offer == null)
                throw new ArgumentNullException();

            authService.restrictAccess(junkyard: offer.junkyard);

            this.copyFromExposed(offer, e_offer);

            unitOfWork.OfferRepository.Update(offer);
            unitOfWork.Save();
        }

        public void deleteOffer(int offer_id)
        {
            OfferEntity offer = unitOfWork.OfferRepository.GetByID(offer_id);

            if (offer == null)
                throw new ArgumentNullException();

            authService.restrictAccess(junkyard: offer.junkyard);

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

            unitOfWork.OfferLineRepository.Update(offer_line);
        }

        public void copyFromExposed(OfferEntity offer, ExpOferta e_offer)
        {
            logService.Info("Empezando a copiar");

            offer.corresponding_id = e_offer.id_en_desguace;
            offer.junkyard         = authService.currentJunkyard();
            offer.status           = OfferStatus.NEW;

            // Remove the old lines, if any
            foreach (var line in offer.lines) 
            {
                unitOfWork.OfferLineRepository.Delete(line.id);
            }
            offer.lines.Clear();

            int order_id = -1;
            // Add the new ones
            foreach (var e_line in e_offer.lineas)
            {
                OfferLineEntity line = new OfferLineEntity();
                this.copyLineFromExposed(line, e_line);
                order_id = line.order_line.order_id;
                line.offer = offer;
                unitOfWork.OfferLineRepository.Insert(line);
            }

            logService.Info("Copiadas las lineas para " + order_id);
            if (order_id != -1)
            {
                OrderEntity order = orderService.getOrder(order_id);
                orderService.updateOrderStatus(order);
            }
            unitOfWork.Save();
            logService.Info("Terminado de copiar " + order_id);
        }

        public void copyLineFromExposed(OfferLineEntity line, ExpOferta.Line e_line)
        {
            line.corresponding_id = e_line.id_en_desguace;
            line.order_line = orderService.getOrderLine(e_line.linea_solicitud_id);
            line.price = e_line.price;
            line.quantity = e_line.quantity;
            line.notes = e_line.notes;
            line.date = e_line.date;
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
                date = line.date,
                selected_ammount = line.selected_ammount
            };

            return e_line;
        }
    }
}