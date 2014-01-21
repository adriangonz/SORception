package com.sorception.jscrap.entities;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.OneToOne;
import javax.persistence.Table;

import org.hibernate.Hibernate;

import com.fasterxml.jackson.annotation.JsonIgnore;

@Entity
@Table(name = "OrderLine")
public class OrderLineEntity extends AbstractEntity {
	@Column(name = "sgId")
	private String _sgId;
	
	@Column(name = "description")
	private String _description;
	
	@Column(name = "quantity")
	private Integer _quantity;
	
	@ManyToOne(fetch = FetchType.EAGER)
	@JoinColumn(name = "orderId", nullable = false)
	private OrderEntity _order;
	
	@OneToOne(fetch = FetchType.EAGER, cascade = CascadeType.ALL,
			mappedBy = "_orderLine")
	private OfferLineEntity _offerLine;
	
	public OrderLineEntity() {}
	
	public OrderLineEntity(String sgId,
			String description,
			Integer quantity) {
		this._sgId = sgId;
		this._description = description;
		this._quantity = quantity;
	}
	
	public void setOrder(OrderEntity order) {
		this._order = order;
	}
	
	public void setOfferLine(OfferLineEntity offerLine) {
		this._offerLine = offerLine;
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
	
	public OfferLineEntity getOfferLine() {
		return _offerLine;
	}
	
	public OrderEntity getOrder() {
		return _order;
	}
}
