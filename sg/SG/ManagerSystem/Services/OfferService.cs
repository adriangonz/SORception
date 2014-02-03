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
            OfferEntity offer = unitOfWork.OfferRepository.GetByID(offer_id);

            if (offer == null)
                throw new ArgumentNullException();

            if (offer.order.garage != garageService.getCurrentGarage())
                throw new ArgumentException();

            return offer;
        }

        public List<OfferEntity> getOffers(int order_id)
        {
            OrderEntity order = orderService.getOrder(order_id);

            return order.offers;
        }

        public ExpOferta toExposed(OfferEntity offer)
        {
            ExpOferta e_offer = new ExpOferta();
            e_offer.id = offer.id;
            e_offer.solicitud_id = offer.order.id;
            // copiar las lineas

            return e_offer;
        }
    }
}