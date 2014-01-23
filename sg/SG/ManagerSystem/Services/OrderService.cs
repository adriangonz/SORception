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
            OrderEntity order = unitOfWork.OrderRepository.GetByID(order_id);

            if (order == null)
                throw new ArgumentNullException();

            if (order.garage != garageService.getCurrentGarage())
                throw new ArgumentException();

            return this.toExposed(order);
        }

        public List<ExpSolicitud> getOrders()
        {
            GarageEntity current_garage = garageService.getCurrentGarage();

            List<ExpSolicitud> orders = (from order in current_garage.orders select this.toExposed(order)).ToList();

            return orders;
        }

        public void putOrder(ExpSolicitud e_order)
        {

        }

        public void deleteOrder(int order_id)
        {

        }

        private ExpSolicitud toExposed(OrderEntity order)
        {
            ExpSolicitud e_order = new ExpSolicitud();

            e_order.deadline = order.deadline;
            e_order.id = order.id;
            e_order.id_en_taller = order.corresponding_id;
            e_order.status = order.status.ToString();
            // copiar las lineas

            return e_order;
        }
    }
}