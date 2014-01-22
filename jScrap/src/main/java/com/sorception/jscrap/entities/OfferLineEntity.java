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

import org.hibernate.Hibernate;
import org.springframework.transaction.annotation.Transactional;

import com.fasterxml.jackson.annotation.JsonIgnore;

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
	
	@OneToOne(fetch = FetchType.EAGER)
	@JoinColumn(name="orderLineId", unique = true)
	private OrderLineEntity _orderLine;
	
	@Column(name = "deleted")
	private Boolean _deleted = false;
	
	@OneToOne(fetch = FetchType.EAGER, cascade = CascadeType.ALL,
			mappedBy = "_offerLine")
	private AcceptedOfferLineEntity _acceptedOfferLine;
	
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
	public Long getOfferId() {
		return this._offer.getId();
	}
	
	@JsonIgnore
	public Boolean isDeleted() {
		return this._deleted;
	}
	
	public AcceptedOfferLineEntity getAcceptedOffer() {
		return this._acceptedOfferLine;
	}
	
	/* Nyapicas */
	public void setAcceptedOffer(AcceptedOfferLineEntity acceptedOffer) {
		this._acceptedOfferLine = acceptedOffer;
		this._acceptedOfferLine.setOfferLine(this);
	}
	
	public void setOrderLine(OrderLineEntity orderLine) {
		this._orderLine = orderLine;
	}
	
	public Long getOrderLineId() {
		return this._orderLine != null ? this._orderLine.getId() : null;
	}
	
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
	
	public void delete() {
		this._deleted = true;
		this._orderLine = null;
	}

	@JsonIgnore
	public OfferEntity getOffer() {
		return _offer;
	}
}
