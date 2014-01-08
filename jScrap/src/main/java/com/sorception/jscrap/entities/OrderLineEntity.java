package com.sorception.jscrap.entities;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;

import org.codehaus.jackson.annotate.JsonBackReference;

@Entity
@Table(name = "OrderLine")
public class OrderLineEntity extends AbstractEntity {
	@Column(name = "sgId")
	private String _sgId;
	
	@Column(name = "description")
	private String _description;
	
	@Column(name = "quantity")
	private Integer _quantity;
	
	@ManyToOne(fetch = FetchType.LAZY)
	private OrderEntity _order;
	
	public OrderLineEntity() {}
	
	public OrderLineEntity(String sgId) {
		this._sgId = sgId;
	}

	public String getSgId() {
		return _sgId;
	}

	public String getDescription() {
		return _description;
	}

	public Integer getQuantity() {
		return _quantity;
	}
}
