package com.sorception.jscrap.services;

import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.google.common.base.Preconditions;
import com.google.common.collect.Lists;
import com.sorception.jscrap.dao.IGenericDAO;
import com.sorception.jscrap.entities.AbstractEntity;

@Service
public abstract class AbstractService<T extends AbstractEntity> {
	
	final Logger logger = LoggerFactory.getLogger(getClass());
	
	protected Class<T> serviceClass;
	
	public AbstractService(final Class<T> classToSet) {
		serviceClass = classToSet;
	}
	
	// Methods for finding
	
    @Transactional(readOnly = true)
    public T findOne(final Long id) {
        return getDao().findOne(id);
    }

    @Transactional(readOnly = true)
    public List<T> findAll() {
    	// TODO: Sort by creationDate
        return Lists.newArrayList(getDao().findAll());
    }
    
    // Methods for updating, creating and deleting
    @Transactional
    public T create(final T entity) {
        final T persistedEntity = getDao().save(entity);

        return persistedEntity;
    }
    
    @Transactional
    public void update(final T entity) {
        getDao().save(entity);
    }
    
    @Transactional
    public void delete(final long id) {
        final T entity = getDao().findOne(id);
        getDao().delete(entity);
    }
    
    // Protected helpers
        
    protected abstract IGenericDAO<T> getDao();
}
