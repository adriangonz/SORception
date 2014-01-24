package com.sorception.jscrap.services;

import java.util.ArrayList;
import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.dao.IGenericDAO;
import com.sorception.jscrap.dao.IOrderDAO;
import com.sorception.jscrap.dao.IOrderLineDAO;
import com.sorception.jscrap.dao.OrderDAO;
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
	private IOrderLineDAO lineDao;
	
	@Override
	protected IGenericDAO<OrderEntity> getDao() {
		return dao;
	}
	
	protected IOrderDAO getOrderDao() {
		return dao;
	}
	
	protected IOrderLineDAO getLineDao() {
		return lineDao;
	}
	
	public List<OrderEntity> getAllOrders() {
		List<OrderEntity> orders = getOrderDao().getOpenedOrders();
		return orders;
	}
	
	public OrderEntity addOrder(String sgId, 
			List<OrderLineEntity> orderLines) {
		OrderEntity orderEntity = new OrderEntity(sgId, orderLines);
		return create(orderEntity);
	}
	
	public OrderEntity addOrder(OrderEntity orderEntity) {
		return create(orderEntity);
	}
	
	public OrderEntity getOrderById(Long id) {
		return findOne(id);
	}

	public OrderLineEntity getOrderLine(Long orderLineId) {
		return getLineDao().findOne(orderLineId);
	}

	public OrderEntity getOrderBySgId(String id) {
		return getOrderDao().findBySgId(id);
	}

	public OrderEntity updateOrder(OrderEntity order) {
		update(order);
		return order;
	}
	
	public void closeOrder(OrderEntity order) {
		OfferService offerService = new OfferService();
		order.setClosed(true);
		updateOrder(order);
		// Delete related offer
		offerService.deleteOffer(order.getOffer());
	}

	public void deleteOrder(OrderEntity order) {
		delete(order);
	}
}
