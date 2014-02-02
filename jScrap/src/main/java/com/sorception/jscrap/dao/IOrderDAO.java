package com.sorception.jscrap.dao;

import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.sorception.jscrap.entities.OrderEntity;
import com.sorception.jscrap.entities.OrderLineEntity;

@Repository
public interface IOrderDAO extends IGenericDAO<OrderEntity> {
	@Query("SELECT DISTINCT o "
			+ "FROM OrderEntity AS o "
				+ "JOIN o.lines AS l "
				+ "LEFT JOIN l.offerLine AS offer "
			+ "WHERE o.closed = FALSE "
				+ "AND o.deleted = FALSE "
				+ "AND l.deleted = FALSE "
				+ "AND offer.acceptedOfferLine IS EMPTY "
			+ "ORDER BY o.created DESC ")
	public List<OrderEntity> getOpenedOrders();
	
	@Query("SELECT DISTINCT l "
			+ "FROM OrderLineEntity AS l "
			+ "WHERE l.id = :id")
	public OrderLineEntity getOrderlineById(@Param("id") Long id);
	
	public OrderEntity findBySgId(String sgId);
}
