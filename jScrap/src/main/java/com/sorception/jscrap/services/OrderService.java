package com.sorception.jscrap.services;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.dao.IGenericDAO;
import com.sorception.jscrap.dao.IOrderDAO;
import com.sorception.jscrap.entities.OrderEntity;
import com.sorception.jscrap.entities.OrderLineEntity;
import com.sorception.jscrap.error.ResourceNotFoundException;

@Service
@Transactional
public class OrderService extends AbstractService<OrderEntity> {
		
	public OrderService() {
		super(OrderEntity.class);
	}

	@Autowired
	private IOrderDAO dao;
	
	@Autowired
	private OfferService offerService;
	
	@Override
	protected IGenericDAO<OrderEntity> getDao() {
		return dao;
	}
	
	protected IOrderDAO getOrderDao() {
		return dao;
	}
	
	public List<OrderEntity> getAllOrders() {
		logger.info("Retrieving all orders.");
		List<OrderEntity> orders = getOrderDao().getOpenedOrders();
		return orders;
	}
	
	public OrderEntity addOrder(String sgId, 
			List<OrderLineEntity> orderLines) {
		logger.info("Adding new order with remote id " + sgId + ".");
		OrderEntity orderEntity = new OrderEntity(sgId, orderLines);
		return create(orderEntity);
	}
	
	public OrderEntity addOrder(OrderEntity orderEntity) {
		logger.info("Adding new order with remote id " + orderEntity.getSgId() + ".");
		return create(orderEntity);
	}
	
	public OrderEntity getOrderById(Long id) {
		return findOne(id);
	}

	public OrderLineEntity getOrderLine(Long orderLineId) {
		logger.info("Retrieving orderline with id " + orderLineId + ".");
		OrderLineEntity line = getOrderDao().getOrderlineById(orderLineId);
		if(line == null || line.isDeleted())
			throw new ResourceNotFoundException("Orderline with id " + Long.toString(orderLineId) + " was not found");
		return line;
	}
	
	public OrderEntity getOrderBySgId(String id) {
//		logger.info("Retrieving order with remote id " + id + ".");
		return getOrderDao().findBySgId(id);
	}

	public OrderEntity updateOrder(OrderEntity order) {
		logger.info("Updating order with id " + order.getId() + ".");
		update(order);
		return order;
	}
	
	public void closeOrder(OrderEntity order) {
		logger.info("Closing order with id " + order.getId() + ".");
		order.setClosed(true);
		updateOrder(order);
		// Delete related offer
		if(order.getOffer() != null)
			offerService.deleteOfferWithoutAMQ(order.getOffer());
	}

	public void deleteOrder(OrderEntity order) {
		logger.info("Deleting order with id " + order.getId() + ".");
		delete(order);
	}

	public OrderLineEntity getOrderLineBySgId(String sgId) {
		logger.info("Retrieving orderline with remote id " + sgId + ".");
		return getOrderDao().findOrderLineBySgId(sgId);
	}
}
