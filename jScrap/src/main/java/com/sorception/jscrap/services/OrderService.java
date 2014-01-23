package com.sorception.jscrap.services;

import java.util.ArrayList;
import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.dao.OrderDAO;
import com.sorception.jscrap.entities.OrderEntity;
import com.sorception.jscrap.entities.OrderLineEntity;
import com.sorception.jscrap.error.ResourceNotFoundException;

@Service
@Transactional
public class OrderService {
	
	final static Logger logger = LoggerFactory.getLogger(OrderService.class);
	
	@Autowired
	private OrderDAO orderDAO;
	
	public List<OrderEntity> getAllOrders() {
		List<OrderEntity> orders = orderDAO.list();
		List<OrderEntity> validOrders = new ArrayList<>();
		for(OrderEntity order : orders) {
			if(!order.getNotAccepted().isEmpty() && !order.isClosed())
				validOrders.add(order);
		}
		
		return validOrders;
	}
	
	public OrderEntity addOrder(String sgId, 
			List<OrderLineEntity> orderLines) {
		OrderEntity orderEntity = new OrderEntity(sgId, orderLines);
		return this.addOrder(orderEntity);
	}
	
	public OrderEntity addOrder(OrderEntity orderEntity) {
		return orderDAO.save(orderEntity);
	}
	
	public OrderEntity getOrderById(Long id) {
		OrderEntity order = orderDAO.get(id);
		if(order == null)
			throw new ResourceNotFoundException("Order with id " + 
					Long.toString(id) + " not found");
		
		return order;
	}

	public OrderLineEntity getOrderLine(Long orderLineId) {
		OrderLineEntity orderLine = orderDAO.getOrderLine(orderLineId);
		if(orderLine == null)
			throw new ResourceNotFoundException("OrderLine with id " + 
					Long.toString(orderLineId) + " not found");
		return orderLine;
	}

	public OrderEntity getOrderBySgId(String id) {
		return orderDAO.getBySgId(id);
	}

	public OrderEntity updateOrder(OrderEntity order) {
		return orderDAO.update(order);
	}
	
	public void closeOrder(OrderEntity order) {
		OfferService offerService = new OfferService();
		order.setClosed(true);
		updateOrder(order);
		// Delete related offer
		offerService.deleteOffer(order.getOffer());
	}

	public void deleteOrder(OrderEntity order) {
		OfferService offerService = new OfferService();
		order.setDeleted(true);
		for(OrderLineEntity line : order.getLines()) {
			line.setDeleted(true);
		}
		updateOrder(order);
		// Delete related offer
		offerService.deleteOffer(order.getOffer());
	}
}
