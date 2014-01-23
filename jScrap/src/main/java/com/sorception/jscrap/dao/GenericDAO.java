package com.sorception.jscrap.dao;

import java.util.List;

import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;

import org.springframework.core.GenericTypeResolver;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.entities.AbstractEntity;
import com.sorception.jscrap.entities.SoftDeletableEntity;

@Repository
@Transactional
public class GenericDAO<T extends AbstractEntity> {
	@PersistenceContext(name = "entityManagerFactory")
	EntityManager entityManager;

	private final Class<T> genericType;
	
	public GenericDAO() {
		genericType = (Class<T>) GenericTypeResolver.resolveTypeArgument(getClass(), GenericDAO.class);
	}
	
	public <T extends SoftDeletableEntity> void delete(T entity) {
		entity.setDeleted(true);
	}
	
	public void delete(T entity) {
		entityManager.remove(entity);
	}
	
	public void persist(T entity) {
		entityManager.persist(entity);
	}
	
	public T getById(Long id) {
		return entityManager.find(genericType, id);
	}
	
	public List<T> getAll() {
		return entityManager
			.createQuery("FROM " + genericType.toString())
			.getResultList();
	}
	
	public void update(T entity) {
		entityManager.merge(entity);
	}
	
	public void saveChanges() {
		entityManager.flush();
		entityManager.clear();
	}
}
