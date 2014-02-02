package com.sorception.jscrap.entities;

import org.hibernate.annotations.Filter;
import org.hibernate.annotations.FilterDef;
import org.hibernate.annotations.Filters;


public interface ISoftDeletable {
	public Boolean isDeleted();
	public void setDeleted(Boolean deleted);
}
