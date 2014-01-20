using ScrapWeb.Entities;
using ScrapWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScrapWeb.Controllers
{
    public class OrderController : ApiController
    {
        private OrderService orderService;

        OrderController()
        {
            orderService = new OrderService();
        }

        // GET api/<controller>
        public IEnumerable<OrderEntity> Get()
        {
            return orderService.getAll();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }
    }
}