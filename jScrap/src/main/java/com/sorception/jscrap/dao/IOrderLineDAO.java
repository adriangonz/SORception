package com.sorception.jscrap.dao;

import java.util.List;

import org.springframework.data.jpa.repository.Query;

import com.sorception.jscrap.entities.OrderLineEntity;

public interface IOrderLineDAO extends IGenericDAO<OrderLineEntity> {
	/*
	@Query("SELECT l "
			+ "FROM OrderLineEntity l "
			+ "WHERE l.order.id = ?1 "
				+ "AND (l.offerLine = NULL OR "
					+ "(l.offerLine <> NULL AND l.offerLine.acceptedOfferLine = NULL))")
	*/
	@Query("SELECT l "
			+ "FROM OrderLineEntity l "
			+ "WHERE l.order.id = ?1 ")
	public List<OrderLineEntity> findNotOffered(Long orderId);
}
