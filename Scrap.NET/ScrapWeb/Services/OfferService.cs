using ScrapWeb.DataAccess;
using ScrapWeb.Entities;
using ScrapWeb.Exceptions;
using ScrapWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ScrapWeb.Services
{
    public class OfferService
    {
        private GenericRepository<OfferEntity> offerRepository;
        private ScrapContext scrapContext;
        private OrderService orderService;
        private AMQService amqService;

        public OfferService()
        {
            scrapContext = new ScrapContext();
            offerRepository = new GenericRepository<OfferEntity>(scrapContext);
            orderService = new OrderService(scrapContext);
            amqService = new AMQService();
        }

        public IEnumerable<OfferEntity> getAll()
        {
            return offerRepository.GetAll("lines");
        }

        public OfferEntity save(DTO.OfferPostDTO offer)
        {
            List<OfferLineEntity> lines = new List<OfferLineEntity>();

            foreach (var line in offer.lines)
            {
                var orderline = orderService.getOrderLine(line.orderLineId);
                if (orderline.offerLine != null)
                    throw new ServiceException("Order line " + line.orderLineId + " already has an offer", HttpStatusCode.BadRequest);
                lines.Add(new OfferLineEntity
                {
                    price = line.price,
                    orderLine = orderline,
                    quantity = line.quantity,
                    notes = line.notes
                });
            }

            OfferEntity offerEntity = new OfferEntity
            {
                lines = lines
            };

            offerRepository.Insert(offerEntity);
            scrapContext.SaveChanges();

            amqService.sendOffer(offerEntity);

            return offerEntity;
        }

        public OfferEntity getById(int id)
        {
            OfferEntity offer = offerRepository.GetByID(id);
            if (offer == null)
                throw new ServiceException("Offer with id " + id + " was not found", HttpStatusCode.NotFound);
            return offer;
        }

        public void delete(int id)
        {
            offerRepository.Delete(this.getById(id));
        }
    }
}