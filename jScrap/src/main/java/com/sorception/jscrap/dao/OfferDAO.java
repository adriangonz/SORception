package com.sorception.jscrap.dao;

import java.util.List;

import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;

import org.hibernate.Hibernate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.entities.OfferEntity;
import com.sorception.jscrap.entities.OfferLineEntity;
import com.sorception.jscrap.entities.OrderEntity;
import com.sorception.jscrap.entities.OrderLineEntity;

@Repository
@Transactional
public class OfferDAO {
	@PersistenceContext(name = "entityManagerFactory")
	EntityManager entityManager;
	
	public List<OfferEntity> list() {
		return this.entityManager
				.createQuery("FROM OfferEntity ORDER BY creationDate DESC")
				.getResultList();
	}
	
	public OfferEntity save(OfferEntity offerEntity) {
		this.entityManager.persist(offerEntity);
		return offerEntity;
	}
	
	public OfferEntity get(Long id) {
		return (OfferEntity)this.entityManager.find(OfferEntity.class, id);
	}
	
	public void deleteOfferLine(OrderLineEntity offerLine) {
		this.entityManager.remove(offerLine);
	}
	
	public OrderLineEntity getOrderLine(OfferLineEntity offerLine) {
		OrderLineEntity orderLine = offerLine.getOrderLine();
		Hibernate.initialize(orderLine);
		return orderLine;
	}
	
	public OrderEntity getOrder(OfferEntity offer) {
		OrderLineEntity orderLine = offer.getLines().get(0).getOrderLine();
		Hibernate.initialize(orderLine);
		return orderLine.getOrder();
	}
}
