package com.sorception.jscrap.dao;

import java.util.List;

import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.entities.OrderEntity;
import com.sorception.jscrap.entities.OrderLineEntity;

@Repository
public interface IOrderDAO extends IGenericDAO<OrderEntity> {
	@Query("SELECT o "
			+ "FROM OrderEntity AS o "
				+ "JOIN o.lines AS l "
				+ "LEFT JOIN l.offerLine AS offer "
			+ "WHERE o.closed = FALSE "
				+ "AND o.deleted = FALSE "				
				+ "AND offer.acceptedOfferLine IS EMPTY "
			+ "ORDER BY o.created DESC ")
	public List<OrderEntity> getOpenedOrders();
	
	public OrderEntity findBySgId(String sgId);
}
