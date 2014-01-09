package com.sorception.jscrap.dao;

import java.util.List;

import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;

import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.entities.OfferEntity;

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
}
