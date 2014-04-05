using ManagerSystem.DataAccess;
using ManagerSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
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

            amqService.publishOrder(this.toExposed(order), AMQSolicitudMessage.Code.New);
            jobService.createJob(order);

            return order.id;
        }

        public OrderEntity getOrder(int order_id)
        {
            OrderEntity order = unitOfWork.OrderRepository.GetByID(order_id, "lines");

            authService.restrictAccess(garage: order.garage);

            if (order == null)
                throw new ArgumentNullException();

            return order;
        }

        public List<OrderEntity> getOrders()
        {
            GarageEntity current_garage = authService.currentGarage();

            List<OrderEntity> orders = new List<OrderEntity>();
            foreach (OrderEntity order in current_garage.orders)
            {
                orders.Add(orderService.getOrder(order.id));
            }

            return orders;
        }

        public void putOrder(ExpSolicitud e_order)
        {
            OrderEntity order = unitOfWork.OrderRepository.GetByID(e_order.id);

            authService.restrictAccess(garage: order.garage);

            if (order == null)
                throw new ArgumentNullException();

            this.copyFromExposed(order, e_order);

            unitOfWork.OrderRepository.Update(order);
            unitOfWork.Save();

            amqService.publishOrder(this.toExposed(order), AMQSolicitudMessage.Code.Update);
            jobService.createJob(order);
        }

        public void deleteOrder(int order_id)
        {
            OrderEntity order = unitOfWork.OrderRepository.GetByID(order_id);

            authService.restrictAccess(garage: order.garage);

            if (order == null)
                throw new ArgumentNullException();

            if (order.garage != authService.currentGarage())
                throw new ArgumentException();

            unitOfWork.OrderRepository.Delete(order);
            unitOfWork.Save();

            amqService.publishOrder(this.toExposed(order), AMQSolicitudMessage.Code.Delete);
        }

        public OrderLineEntity getOrderLine(int line_id)
        {
            OrderLineEntity line = unitOfWork.OrderLineRepository.GetByID(line_id);

            if (line == null)
                throw new ArgumentException();
            /*
             * No se si esto deberia quedarse comentado o anyadir una comprobacion 
             * para que los desguaces puedan verlo.
            if (line.order.garage != garageService.getCurrentGarage())
                throw new ArgumentException();
            */
            return line;
        }

        public void updateOrderStatus(OrderEntity order)
        {
            foreach (var order_line in order.lines)
            {
                this.updateOrderLineStatus(order_line);
            }

            if (order.offers.Count > 0)
            {
                order.status = OrderStatus.HAS_RESPONSE;
                unitOfWork.OrderRepository.Update(order);
                unitOfWork.Save();
            }
        }

        public void updateOrderLineStatus(OrderLineEntity order_line)
        {
            OrderLineStatus previous_status = order_line.status;
            if (order_line.selected_ammount >= order_line.quantity)
                order_line.status = OrderLineStatus.COMPLETE;
            else if (order_line.selected_ammount >= 0)
                order_line.status = OrderLineStatus.INCOMPLETE;
            else if (order_line.offers.Count > 0)
                order_line.status = OrderLineStatus.HAS_RESPONSE;

            if (order_line.status != previous_status)
            {
                unitOfWork.OrderLineRepository.Update(order_line);
            }

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
            order.garage = authService.currentGarage();
            order.deadline = e_order.deadline;
            order.corresponding_id = e_order.id_en_taller;
            foreach (var e_line in e_order.lineas)
            {
                OrderLineEntity line;
                switch (e_line.action)
                {
                    case "NEW":
                        line = new OrderLineEntity();
                        this.copyLineFromExposed(line, e_line);
                        line.order = order;
                        unitOfWork.OrderLineRepository.Insert(line);
                        break;
                    case "UPDATED":
                        line = this.getOrderLine(e_line.id);
                        if (line.order_id != order.id)
                            throw new ArgumentException();
                        this.copyLineFromExposed(line, e_line);
                        unitOfWork.OrderLineRepository.Update(line);
                        break;
                    case "DELETED":
                        line = this.getOrderLine(e_line.id);
                        if (line.order != order)
                            throw new ArgumentException();
                        unitOfWork.OrderLineRepository.Delete(line);
                        break;
                    default:
                        throw new ArgumentException(String.Format("Action {0} for order line {1} not valid. Must be NEW, UPDATED or DELETED", e_line.action, e_line.id_en_taller));
                }
            }
        }

        public ExpSolicitud toExposed(OrderEntity order)
        {
            ExpSolicitud e_order = new ExpSolicitud();

            e_order.deadline     = order.deadline;
            e_order.id           = order.id;
            e_order.id_en_taller = order.corresponding_id;
            e_order.status       = order.status.ToString();
            e_order.lineas       = new List<ExpSolicitud.Line>();
            foreach (var order_line in order.lines)
            {
                if (!order_line.deleted) // TODO
                    e_order.lineas.Add(this.toExposed(order_line));
            }

            return e_order;
        }

        public ExpSolicitud.Line toExposed(OrderLineEntity order_line)
        {
            ExpSolicitud.Line e_line = new ExpSolicitud.Line();

            e_line.id = order_line.id;
            e_line.description = order_line.description;
            e_line.quantity = order_line.quantity;
            e_line.status = order_line.status.ToString();
            e_line.flag = order_line.flag.ToString();
            e_line.id_en_taller = order_line.corresponding_id;

            return e_line;
        }

        public List<OfferLineEntity> getOrderedOfferLines(OrderLineEntity order_line)
        {
            switch (order_line.flag)
            {
                case OrderLineFlag.CHEAPEST:
                    return order_line.offers.OrderBy(o_l => o_l.price).ToList();
                /*case OrderLineFlag.NEWEST:
                    return order_line.offers.OrderBy(o_l => o_l.year).ToList();*/
                default:
                    return order_line.offers.ToList();
            }
        }
    }
}