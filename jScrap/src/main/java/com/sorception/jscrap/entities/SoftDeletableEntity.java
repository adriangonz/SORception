package com.sorception.jscrap.entities;

import javax.persistence.Column;
import javax.persistence.MappedSuperclass;

import org.hibernate.annotations.SQLDelete;
import org.hibernate.annotations.Where;

import com.fasterxml.jackson.annotation.JsonIgnore;

@MappedSuperclass
@Where(clause="deleted <> '1'")
public abstract class SoftDeletableEntity {
	@Column(name = "deleted")
	private boolean _deleted;
	
	@JsonIgnore
	public boolean isDeleted() {
		return _deleted;
	}
	
	public void setDeleted(boolean deleted) {
		_deleted = deleted;
	}
}
