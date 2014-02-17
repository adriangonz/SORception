package com.sorception.jscrap.dao;

import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.sorception.jscrap.entities.OfferEntity;
import com.sorception.jscrap.entities.OfferLineEntity;

@Repository
public interface IOfferDAO extends IGenericDAO<OfferEntity>{

	@Query("SELECT DISTINCT o "
			+ "FROM OfferEntity AS o "
				+ "JOIN o.lines AS l "
				+ "JOIN l.orderLine AS ol "
				+ "JOIN ol.order AS order "
			+ "WHERE l.acceptedOfferLine IS NOT EMPTY "
				+ "OR order.closed = TRUE "
			+ "ORDER BY o.created DESC")
	public List<OfferEntity> getAcceptedOffers();
	
	@Query("SELECT DISTINCT o "
			+ "FROM OfferEntity as o "
				+ "JOIN o.lines AS l "
				+ "JOIN l.orderLine AS ol "
				+ "JOIN ol.order AS order "
			+ "WHERE o.deleted = FALSE "
				+ "AND order.deleted = FALSE "
				+ "AND order.closed = FALSE "
			+ "ORDER BY o.created DESC")
	public List<OfferEntity> getOpenedOffers();
	
	@Query("SELECT DISTINCT l "
			+ "FROM OfferLineEntity AS l "
			+ "WHERE l.id = :id "
				+ "AND l.deleted = FALSE")
	public OfferLineEntity getOfferlineById(@Param("id") Long id);
}
