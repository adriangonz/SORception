package com.sorception.jscrap.entities;

import org.hibernate.annotations.FilterDef;

@FilterDef(name="softDelete", defaultCondition="deleted = false")
public interface ISoftDeletable {
	public Boolean isDeleted();
	public void setDeleted(Boolean deleted);
}
