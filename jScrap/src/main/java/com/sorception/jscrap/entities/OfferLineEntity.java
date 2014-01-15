package com.sorception.jscrap.entities;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.OneToOne;
import javax.persistence.Table;
import javax.persistence.Transient;

import org.codehaus.jackson.annotate.JsonIgnore;
import org.hibernate.Hibernate;
import org.springframework.transaction.annotation.Transactional;

@Entity
@Table(name = "OfferLine")
public class OfferLineEntity extends AbstractEntity {
	
	@Column(name = "quantity")
	private Integer _quantity;
	
	@Column(name = "notes")
	private String _notes;
	
	@Column(name = "price")
	private Double _price;
	
	@ManyToOne(fetch = FetchType.EAGER, cascade = CascadeType.ALL)
	@JoinColumn(name = "offerId", nullable = false)
	private OfferEntity _offer;
	
	@OneToOne(fetch = FetchType.LAZY)
	@JoinColumn(name="orderLineId", unique = true)
	private OrderLineEntity _orderLine;
	
	@Transient
	private Boolean _toDelete = false;
	
	public OfferLineEntity() {}
	
	public OfferLineEntity(Integer quantity,
			String notes,
			Double price,
			OrderLineEntity orderLine) {
		this._quantity = quantity;
		this._notes = notes;
		this._price = price;
		this._orderLine = orderLine;
		this._orderLine.setOfferLine(this);
	}
	
	public void setOffer(OfferEntity offer) {
		this._offer = offer;
	}
	
	public Double getPrice() {
		return _price;
	}

	public Integer getQuantity() {
		return _quantity;
	}

	public String getNotes() {
		return _notes;
	}
	
	/* Nyapicas */
	public void setOrderLine(OrderLineEntity orderLine) {
		this._orderLine = orderLine;
	}
	
	@JsonIgnore
	public OrderLineEntity getOrderLine() {
		return _orderLine;
	}

	public void setQuantity(Integer quantity) {
		this._quantity = quantity;
	}

	public void setNotes(String notes) {
		this._notes = notes;
	}

	public void setPrice(Double price) {
		this._price = price;
	}
	
	public void markToDelete() {
		this._toDelete = true;
	}
	
	@JsonIgnore
	public Boolean toDelete() {
		return this._toDelete;
	}
}
