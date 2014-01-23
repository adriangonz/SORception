using ManagerSystem.DataAccess;
using ManagerSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerSystem.Services
{
    public class OrderService : BaseService
    {
        public OrderService(UnitOfWork uow = null) : base(uow) { }

        public int addOrder(ExpSolicitud e_order)
        {
            OrderEntity order = new OrderEntity();

            order.garage = garageService.getCurrentGarage();
            order.deadline = e_order.deadline;
            order.corresponding_id = e_order.id_en_taller;

            unitOfWork.OrderRepository.Insert(order);
            unitOfWork.Save();

            return order.id;
        }

        public ExpSolicitud getOrder(int order_id)
        {
            return new ExpSolicitud();
        }

        public List<ExpSolicitud> getOrders()
        {
            return new List<ExpSolicitud>();
        }

        public void putOrder(ExpSolicitud e_order)
        {

        }

        public void deleteOrder(int order_id)
        {

        }
    }
}