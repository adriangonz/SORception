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
			+ "FROM OrderEntity o "
			+ "WHERE o.closed = FALSE "
			+ "ORDER BY created DESC ")
	public List<OrderEntity> getOpenedOrders();
	
	public OrderEntity findBySgId(String sgId);
}
