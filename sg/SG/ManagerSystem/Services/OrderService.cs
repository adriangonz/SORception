using ManagerSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem.Services
{
    public class OrderService : BaseService
    {
        public OrderService(UnitOfWork uow = null) : base(uow) { }

        public ExpSolicitud getOrder(int order_id)
        {
            return new ExpSolicitud();
        }

        public List<ExpSolicitud> getOrders()
        {
            return new List<ExpSolicitud>();
        }

        public int addOrder(ExpSolicitud e_order)
        {

        }

        public void putOrder(ExpSolicitud e_order)
        {

        }

        public void deleteOrder(int order_id)
        {

        }
    }
}