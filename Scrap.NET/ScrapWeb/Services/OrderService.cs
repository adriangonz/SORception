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
    public class OrderService
    {
        private LogsRepository Logs;

        private GenericRepository<OrderEntity> orderRepository;

        private GenericRepository<OrderLineEntity> orderLineRepository;

        private OfferService offerService;

        private ScrapContext scrapContext;

        public OrderService(ScrapContext context = null)
        {
            if (context == null)
                context = new ScrapContext();
            this.scrapContext = context;
            orderRepository = new GenericRepository<OrderEntity>(scrapContext);
            orderLineRepository = new GenericRepository<OrderLineEntity>(scrapContext);
            Logs = new LogsRepository(scrapContext);
        }

        public IEnumerable<OrderEntity> getAll()
        {
            return orderRepository
                .Get(t => !t.closed && !t.deleted, null, "rawLines")
                .Where(t => t.lines.Count() > 0);
        }

        public OrderEntity save(OrderEntity orderEntity)
        {
            orderRepository.Insert(orderEntity);
            Logs.create(LogEntity.INFO, "Create Order with id " + orderEntity.id);
            scrapContext.SaveChanges();
            return orderEntity;
        }

        public OrderEntity update(OrderEntity orderEntity)
        {
            orderRepository.Update(orderEntity);
            scrapContext.SaveChanges();
            Logs.create(LogEntity.INFO, "Updated Order with id " + orderEntity.id);
            return orderEntity;
        }

        public OrderEntity getBySgId(string sgId)
        {
            var orderentity = orderRepository.Get(t => t.sgId == sgId, null, null).FirstOrDefault();
            if (orderentity == null)
            {
                Logs.create(LogEntity.ERROR, "Error while trying to get order with remote id " + orderentity.id);
                throw new ServiceException("Order with remote id " + sgId + " was not found", HttpStatusCode.NotFound);
            }
            return orderentity;
        }

        public OrderEntity getById(int id)
        {
            var orderEntity = orderRepository.Get(t => t.id == id, null, "rawLines").FirstOrDefault();
            if (orderEntity == null)
            {
                Logs.create(LogEntity.ERROR, "Error while trying to get order with id " + orderEntity.id);
                throw new ServiceException("Order with id " + id.ToString() + " was not found", HttpStatusCode.NotFound);
            }
            return orderEntity;
        }

        public OrderLineEntity getOrderLine(int id)
        {
            var orderLine = orderLineRepository.GetByID(id);
            if (orderLine == null)
            {
                Logs.create(LogEntity.ERROR, "Error while trying to get order line with id " + orderLine.id);
                throw new ServiceException("Orderline with id " + id.ToString() + " was not found", HttpStatusCode.NotFound);

            }
            return orderLine;
        }

        public void closeOrder(OrderEntity orderEntity)
        {
            orderEntity.closed = true;
            this.update(orderEntity);
            var offerEntity = orderEntity.offer;
            if (offerEntity != null)
                offerService.delete(offerEntity);
            Logs.create(LogEntity.INFO, "Closed order with id " + orderEntity.id);
        }

        public void deleteOrder(OrderEntity orderEntity)
        {
            var offerService = new OfferService(scrapContext);
            orderEntity.deleted = true;
            foreach(OrderLineEntity line in orderEntity.rawLines) {
                line.deleted = true;
            }
            this.update(orderEntity);
            var offerEntity = orderEntity.offer;
            if(offerEntity != null)
                offerService.delete(offerEntity);
            Logs.create(LogEntity.INFO, "Deleted order with id " + orderEntity.id);
        }

        public void deleteOrderLine(OrderLineEntity orderLine)
        {
            orderLine.deleted = true;
            orderLineRepository.Update(orderLine);
            Logs.create(LogEntity.INFO, "Deleted order line with id " + orderLine.id);
            scrapContext.SaveChanges();
        }
    }
}