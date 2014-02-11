package com.sorception.jscrap.data;

import java.util.Set;

import org.hibernate.HibernateException;
import org.hibernate.engine.spi.EntityEntry;
import org.hibernate.event.internal.DefaultDeleteEventListener;
import org.hibernate.event.spi.DeleteEvent;
import org.hibernate.persister.entity.EntityPersister;
import org.springframework.stereotype.Component;

import com.sorception.jscrap.entities.ISoftDeletable;

@Component
public class SoftDeleteEventListener extends DefaultDeleteEventListener {
	@Override
	public void onDelete(DeleteEvent event, Set arg1) throws HibernateException {
	    Object o = event.getObject();
	    if (o instanceof ISoftDeletable) {
	        EntityPersister persister = event.getSession().getEntityPersister( event.getEntityName(), o);
	        EntityEntry entityEntry = event.getSession().getPersistenceContext().getEntry(o);
	        cascadeBeforeDelete(event.getSession(), persister, o, entityEntry, arg1);
	        ((ISoftDeletable)o).setDeleted(true);
	        cascadeAfterDelete(event.getSession(), persister, o, arg1);
	    } else {
	        super.onDelete(event, arg1);
	    }
	}
}
