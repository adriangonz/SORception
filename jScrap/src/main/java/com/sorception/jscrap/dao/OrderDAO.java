package com.sorception.jscrap.dao;

import java.util.ArrayList;
import java.util.List;

import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;

import org.hibernate.Hibernate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.entities.OrderEntity;
import com.sorception.jscrap.entities.OrderLineEntity;
import com.sorception.jscrap.entities.UserEntity;

@Repository
@Transactional
public class OrderDAO {
	@PersistenceContext(name = "entityManagerFactory")
    EntityManager entityManager;
	
	public List<OrderEntity> list() {
		return this.entityManager
				.createQuery("FROM OrderEntity ORDER BY creationDate DESC")
				.getResultList();
	}
	
	public OrderEntity save(OrderEntity orderEntity) {
		this.entityManager.persist(orderEntity);
		return orderEntity;
	}
	
	public OrderEntity update(OrderEntity orderEntity) {
		return entityManager.merge(orderEntity);
	}
	
	public OrderEntity get(Long id) {
		return (OrderEntity)this.entityManager.find(OrderEntity.class, id);
	}

	public OrderLineEntity getOrderLine(Long orderLineId) {
		return (OrderLineEntity)this.entityManager.find(
				OrderLineEntity.class, orderLineId);
	}
	
	public OrderEntity getOrder(OrderLineEntity orderLine) {
		OrderEntity order = orderLine.getOrder();
		Hibernate.initialize(order);
		return order;
	}

	public OrderEntity getBySgId(String sgId) {
		List<OrderEntity> orders = this.entityManager
                .createQuery("from OrderEntity where sgId = :sgId")
                .setParameter("sgId", sgId)
                .getResultList();
        if(orders.isEmpty())
            return null;
        else
            return orders.get(0);
	}
}
