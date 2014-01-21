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
        private GenericRepository<OfferLineEntity> offerLineRepository;
        private ScrapContext scrapContext;
        private OrderService orderService;
        private AMQService amqService;

        public OfferService()
        {
            scrapContext = new ScrapContext();
            offerRepository = new GenericRepository<OfferEntity>(scrapContext);
            offerLineRepository = new GenericRepository<OfferLineEntity>(scrapContext);
            orderService = new OrderService(scrapContext);
            amqService = new AMQService();
        }

        public IEnumerable<OfferEntity> getAll()
        {
            return offerRepository.Get(t => !t.deleted, null, "rawLines");
        }

        public OfferEntity update(int id, DTO.OfferPostDTO offer)
        {
            OfferEntity offerEntity = this.getById(id);
            foreach (var line in offer.lines.ToList())
            {
                if (line.status == "DELETE")
                {
                    deleteOfferLine(line.id);
                    offer.lines.Remove(line);
                }
                else if (line.status == "NEW") 
                {
                    offerEntity.rawLines.Add(toOfferLine(line));
                }
                else if (line.status == "UPDATE") 
                {
                    var updatedLine = getOfferLine(line.id);
                    updatedLine.notes = line.notes;
                    updatedLine.price = line.price;
                    updatedLine.quantity = line.quantity;
                    offerLineRepository.Update(updatedLine);
                }
            }
            scrapContext.SaveChanges();
            return offerEntity;
        }

        public OfferEntity save(DTO.OfferPostDTO offer)
        {
            List<OfferLineEntity> lines = new List<OfferLineEntity>();

            foreach (var line in offer.lines)
            {
                lines.Add(toOfferLine(line));
            }

            OfferEntity offerEntity = new OfferEntity
            {
                rawLines = lines
            };

            offerRepository.Insert(offerEntity);
            scrapContext.SaveChanges();

            amqService.sendOffer(offerEntity);

            return offerEntity;
        }

        public OfferEntity getById(int id)
        {
            OfferEntity offer = offerRepository.Get(t => t.id == id && !t.deleted, null, "rawLines").FirstOrDefault();
            if (offer == null)
                throw new ServiceException("Offer with id " + id + " was not found", HttpStatusCode.NotFound);
            return offer;
        }

        public void delete(int id)
        {
            var offerEntity = this.getById(id);
            offerEntity.deleted = true;
            offerRepository.Update(offerEntity);
            foreach (var line in offerEntity.rawLines) 
            {
                delete(line);
            }
            scrapContext.SaveChanges();
        }

        private void delete(OfferLineEntity offerLine) 
        {
            delete(offerLine.id);
        }

        private OfferLineEntity getOfferLine(int id)
        {
            var line = offerLineRepository.Get(t => t.id == id && !t.deleted, null, "orderLine").FirstOrDefault();
            if (line == null)
                throw new ServiceException("Offerline with id " + id + " was not found", HttpStatusCode.NotFound);
            return line;
        }

        private void deleteOfferLine(int id) 
        {
            var line = this.getOfferLine(id);
            line.deleted = true;
            offerLineRepository.Update(line);
        }

        private OfferLineEntity toOfferLine(DTO.OfferLineDTO line)
        {
            var orderline = orderService.getOrderLine(line.orderLineId);
            if (orderline.offerLine != null)
                throw new ServiceException("Order line " + line.orderLineId + " already has an offer", HttpStatusCode.BadRequest);
            return new OfferLineEntity
            {
                price = line.price,
                orderLine = orderline,
                quantity = line.quantity,
                notes = line.notes
            };
        }
    }
}