package com.sorception.jscrap.services;

import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.google.common.collect.Lists;
import com.sorception.jscrap.dao.IGenericDAO;
import com.sorception.jscrap.entities.AbstractEntity;
import com.sorception.jscrap.entities.ISoftDeletable;
import com.sorception.jscrap.error.ResourceNotFoundException;

@Service
public abstract class AbstractService<T extends AbstractEntity> {
	
	final Logger logger = LoggerFactory.getLogger(getClass());
	
	protected Class<T> serviceClass;
	
	public AbstractService(final Class<T> classToSet) {
		serviceClass = classToSet;
	}
	
	// Methods for finding
	
    @Transactional(readOnly = true)
    protected T findOne(final Long id) {
        T entity = getDao().findOne(id);
        if(entity == null)
        	throw new ResourceNotFoundException(serviceClass.getSimpleName() + " with id " + id + " was not found");
        else if(entity instanceof ISoftDeletable 
        		&& ((ISoftDeletable)entity).isDeleted())
        	throw new ResourceNotFoundException(serviceClass.getSimpleName() + " has been deleted");
        return entity;
    }

    @Transactional(readOnly = true)
    protected List<T> findAll() {
        return Lists.newArrayList(getDao().findAll(new Sort(Sort.Direction.DESC, "created")));
    }
    
    // Methods for updating, creating and deleting
    @Transactional
    protected T create(final T entity) {
        final T persistedEntity = getDao().save(entity);

        return persistedEntity;
    }
    
    @Transactional
    protected void update(final T entity) {
        getDao().save(entity);
    }
    
    @Transactional
    protected void delete(final long id) {
        final T entity = findOne(id);
        delete(entity);
    }
    
    @Transactional
    protected void delete(final T entity) {
    	getDao().delete(entity);
    }
    
    // Protected helpers
    protected abstract IGenericDAO<T> getDao();
}
