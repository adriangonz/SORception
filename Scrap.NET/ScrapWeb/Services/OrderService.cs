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
        private GenericRepository<OrderEntity> orderRepository;

        private GenericRepository<OrderLineEntity> orderLineRepository;

        private ScrapContext scrapContext;

        public OrderService(ScrapContext context = null)
        {
            if (context == null)
                context = new ScrapContext();
            this.scrapContext = context;
            orderRepository = new GenericRepository<OrderEntity>(scrapContext);
            orderLineRepository = new GenericRepository<OrderLineEntity>(scrapContext);
        }

        public IEnumerable<OrderEntity> getAll()
        {
            return orderRepository
                .Get(t => !t.closed, null, "rawLines")
                .Where(t => t.lines.Count() > 0);
        }

        public OrderEntity save(OrderEntity orderEntity)
        {
            orderRepository.Insert(orderEntity);
            scrapContext.SaveChanges();
            return orderEntity;
        }

        public OrderEntity update(OrderEntity orderEntity)
        {
            orderRepository.Update(orderEntity);
            scrapContext.SaveChanges();
            return orderEntity;
        }

        public OrderEntity getBySgId(string sgId)
        {
            var orderentity = orderRepository.Get(t => t.sgId == sgId, null, null).FirstOrDefault();
            if(orderentity == null)
                throw new ServiceException("Order with remote id " + sgId + " was not found", HttpStatusCode.NotFound);
            return orderentity;
        }

        public OrderEntity getById(int id)
        {
            var orderEntity = orderRepository.GetByID(id);
            if (orderEntity == null)
                throw new ServiceException("Order with id " + id.ToString() + " was not found", HttpStatusCode.NotFound);
            return orderEntity;
        }

        public OrderLineEntity getOrderLine(int id)
        {
            var orderLine = orderLineRepository.GetByID(id);
            if (orderLine == null)
                throw new ServiceException("Orderline with id " + id.ToString() + " was not found", HttpStatusCode.NotFound);
            return orderLine;
        }
    }
}