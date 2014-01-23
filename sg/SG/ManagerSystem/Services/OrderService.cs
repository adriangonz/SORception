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

            this.copyFromExposed(order, e_order);

            unitOfWork.OrderRepository.Insert(order);
            unitOfWork.Save();

            AMQSolicitudMessage msg = new AMQSolicitudMessage
            {
                code = AMQSolicitudMessage.Code.New,
                solicitud = this.toExposed(order)
            };
            amqService.publishOrder(msg);

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

            List<ExpSolicitud> orders = (from order in current_garage.orders where !order.deleted select this.toExposed(order)).ToList();

            return orders;
        }

        public void putOrder(ExpSolicitud e_order)
        {
            OrderEntity order = unitOfWork.OrderRepository.GetByID(e_order.id);

            if (order == null)
                throw new ArgumentNullException();

            if (order.garage != garageService.getCurrentGarage())
                throw new ArgumentException();

            this.copyFromExposed(order, e_order);

            unitOfWork.OrderRepository.Update(order);
            unitOfWork.Save();
        }

        public void deleteOrder(int order_id)
        {
            OrderEntity order = unitOfWork.OrderRepository.GetByID(order_id);

            if (order == null)
                throw new ArgumentNullException();

            if (order.garage != garageService.getCurrentGarage())
                throw new ArgumentException();

            unitOfWork.OrderRepository.Delete(order);
            unitOfWork.Save();
        }

        private void copyFromExposed(OrderEntity order, ExpSolicitud e_order)
        {
            order.garage = garageService.getCurrentGarage();
            order.deadline = e_order.deadline;
            order.corresponding_id = e_order.id_en_taller;
            // copiar las lineas
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