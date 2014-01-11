package com.sorception.jscrap.entities;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.OneToOne;
import javax.persistence.Table;

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
	
	@ManyToOne(fetch = FetchType.EAGER)
	@JoinColumn(name = "offerId", nullable = false)
	private OfferEntity _offer;
	
	@OneToOne(fetch = FetchType.LAZY, optional = false)
	@JoinColumn(name="orderLineId")
	private OrderLineEntity _orderLine;
	
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
	
	@JsonIgnore
	public OrderLineEntity getOrderLine() {
		return _orderLine;
	}
}
