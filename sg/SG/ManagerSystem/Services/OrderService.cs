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

        public OrderEntity getOrder(int order_id)
        {
            OrderEntity order = unitOfWork.OrderRepository.GetByID(order_id);

            if (order == null)
                throw new ArgumentNullException();

            if (order.garage != garageService.getCurrentGarage())
                throw new ArgumentException();

            return order;
        }

        public List<OrderEntity> getOrders()
        {
            GarageEntity current_garage = garageService.getCurrentGarage();

            List<OrderEntity> orders = (from order in current_garage.orders where !order.deleted select order).ToList();

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

            AMQSolicitudMessage msg = new AMQSolicitudMessage
            {
                code = AMQSolicitudMessage.Code.Update,
                solicitud = this.toExposed(order)
            };
            amqService.publishOrder(msg);
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

            AMQSolicitudMessage msg = new AMQSolicitudMessage
            {
                code = AMQSolicitudMessage.Code.Delete,
                solicitud = new ExpSolicitud
                {
                    id = order.id
                }
            };
            amqService.publishOrder(msg);
        }

        private void copyLineFromExposed(OrderLineEntity order_line, ExpSolicitud.Line e_order_line)
        {
            order_line.corresponding_id = e_order_line.id_en_taller;
            order_line.description = e_order_line.description;
            order_line.quantity = e_order_line.quantity;
            order_line.setFlag(e_order_line.flag);
            order_line.status = OrderLineStatus.NEW;
        }

        private void copyFromExposed(OrderEntity order, ExpSolicitud e_order)
        {
            order.garage = garageService.getCurrentGarage();
            order.deadline = e_order.deadline;
            order.corresponding_id = e_order.id_en_taller;
            // copiar las lineas
        }

        public ExpSolicitud toExposed(OrderEntity order)
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