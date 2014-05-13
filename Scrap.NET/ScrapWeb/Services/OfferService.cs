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
        private LogsRepository Logs;
        private GenericRepository<OfferEntity> offerRepository;
        private GenericRepository<OfferLineEntity> offerLineRepository;
        private ScrapContext scrapContext;
        private OrderService orderService;
        private AMQService amqService;

        public OfferService(ScrapContext context = null)
        {
            if (context == null)
                scrapContext = new ScrapContext();
            else
                scrapContext = context;
            offerRepository = new GenericRepository<OfferEntity>(scrapContext);
            offerLineRepository = new GenericRepository<OfferLineEntity>(scrapContext);
            orderService = new OrderService(scrapContext);
            amqService = new AMQService();
            Logs = new LogsRepository(scrapContext);
        }

        public IEnumerable<OfferEntity> getAll()
        {
            return offerRepository.Get(t => !t.deleted, null, "rawLines");
        }

        public IEnumerable<OfferEntity> getAccepted()
        {
            return getAll().Where(t => t.accepted.Count() > 0);
        }

        public List<OfferLineEntity> update(List<OfferLineEntity> acceptedOffers) 
        {
            foreach (OfferLineEntity offerline in acceptedOffers)
                updateOfferLine(offerline);
            return acceptedOffers;
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
                    updateOfferLine(updatedLine);
                }
            }
            Logs.create(LogEntity.INFO, "Updated offer with id " + offerEntity.id);
            scrapContext.SaveChanges();
            amqService.updateOffer(offerEntity);
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
            Logs.create(LogEntity.INFO, "Created offer");
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
        
            delete(offerEntity);
        }

        public void delete(OfferEntity offerEntity)
        {
            offerEntity.deleted = true;
            offerRepository.Update(offerEntity);
            foreach (var line in offerEntity.rawLines)
            {
                delete(line);
            }


            Logs.create(LogEntity.INFO, "Deleted offer with id " + offerEntity.id);
            scrapContext.SaveChanges();
            amqService.deleteOffer(offerEntity);
        }

        private void delete(OfferLineEntity offerLine)
        {
            delete(offerLine.id);
        }

        public OfferLineEntity getOfferLine(int id)
        {
            var line = offerLineRepository.Get(t => t.id == id && !t.deleted, null, "orderLine,offer").FirstOrDefault();
            if (line == null)
                throw new ServiceException("Offerline with id " + id + " was not found", HttpStatusCode.NotFound);
            return line;
        }

        public OfferLineEntity updateOfferLine(OfferLineEntity offerLine)
        {
            offerLineRepository.Update(offerLine);
            scrapContext.SaveChanges();
            return offerLine;
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
                notes = line.notes,
                date = line.date
            };
        }
    }
}