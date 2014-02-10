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