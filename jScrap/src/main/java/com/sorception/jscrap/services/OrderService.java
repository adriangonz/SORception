package com.sorception.jscrap.services;

import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.dao.OrderDAO;
import com.sorception.jscrap.entities.OrderEntity;
import com.sorception.jscrap.entities.OrderLineEntity;

@Service
@Transactional
public class OrderService {
	
	final static Logger logger = LoggerFactory.getLogger(OrderService.class);
	
	@Autowired
	private OrderDAO orderDAO;
	
	public List<OrderEntity> getAllOrders() {
		return orderDAO.list();
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
		return orderDAO.get(id);
	}
}
