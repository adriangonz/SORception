using ScrapWeb.DataAccess;
using ScrapWeb.Entities;
using ScrapWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrapWeb.Services
{
    public class OrderService
    {
        private GenericRepository<OrderEntity> orderRepository;

        private GenericRepository<OrderLineEntity> orderLineRepository;

        private ScrapContext scrapContext;

        public OrderService()
        {
            scrapContext = new ScrapContext();
            orderRepository = new GenericRepository<OrderEntity>(scrapContext);
            orderLineRepository = new GenericRepository<OrderLineEntity>(scrapContext);
        }

        public IEnumerable<OrderEntity> getAll()
        {
            return orderRepository.GetAll("lines");
        }

        public OrderEntity save(OrderEntity orderEntity)
        {
            orderRepository.Insert(orderEntity);
            scrapContext.SaveChanges();
            return orderEntity;
        }
    }
}