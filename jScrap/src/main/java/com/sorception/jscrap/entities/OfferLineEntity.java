package com.sorception.jscrap.entities;

import java.util.Date;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.OneToOne;
import javax.persistence.Table;

import com.fasterxml.jackson.annotation.JsonIgnore;

@Entity
@Table(name = "OfferLine")
public class OfferLineEntity extends AbstractEntity implements ISoftDeletable {
	
	@Column(name = "quantity")
	private Integer quantity;
	
	@Column(name = "notes")
	private String notes;
	
	@Column(name = "price")
	private Double price;
	
	@Column(name = "date")
	private Date date;
	
	@ManyToOne(fetch = FetchType.EAGER, cascade = CascadeType.PERSIST)
	@JoinColumn(name = "offerId")
	private OfferEntity offer;
	
	@ManyToOne(fetch = FetchType.EAGER)
	@JoinColumn(name="orderLineId", unique = true)
	private OrderLineEntity orderLine;
	
	@Column(name = "deleted", nullable = false)
	private Boolean deleted;
	
	@OneToOne(fetch = FetchType.EAGER,
			mappedBy = "offerLine",
			cascade = CascadeType.ALL)
	private AcceptedOfferLineEntity acceptedOfferLine;
	
	public OfferLineEntity() {}
	
	public OfferLineEntity(Integer quantity,
			String notes,
			Double price,
			Date date,
			OrderLineEntity orderLine) {
		this.quantity = quantity;
		this.notes = notes;
		this.price = price;
		this.date = date;
		this.orderLine = orderLine;
		this.orderLine.setOfferLine(this);
		this.deleted = false;
	}
	
	public Double getPrice() {
		return price;
	}

	public Integer getQuantity() {
		return quantity;
	}

	public String getNotes() {
		return notes;
	}
	
	public Date getDate() {
		return date;
	}
	
	@JsonIgnore
	public Long getOfferId() {
		return this.offer.getId();
	}
	
	@JsonIgnore
	public Boolean isDeleted() {
		return this.deleted;
	}
	
	public AcceptedOfferLineEntity getAcceptedOffer() {
		return this.acceptedOfferLine;
	}
	
	public Long getOrderLineId() {
		return this.orderLine != null ? this.orderLine.getId() : null;
	}
	
	public OrderLineEntity getOrderLine() {
		return orderLine;
	}
	
	@JsonIgnore
	public OfferEntity getOffer() {
		return offer;
	}
	
	public void setAcceptedOffer(AcceptedOfferLineEntity acceptedOffer) {
		this.acceptedOfferLine = acceptedOffer;
		this.acceptedOfferLine.setOfferLine(this);
	}
	
	public void setOrderLine(OrderLineEntity orderLine) {
		this.orderLine = orderLine;
	}

	public void setQuantity(Integer quantity) {
		this.quantity = quantity;
	}

	public void setNotes(String notes) {
		this.notes = notes;
	}

	public void setPrice(Double price) {
		this.price = price;
	}
	
	public void setDate(Date date) {
		this.date = date;
	}
	
	public void setOffer(OfferEntity offer) {
		this.offer = offer;
	}

	@Override
	public void setDeleted(Boolean deleted) {
		this.deleted = deleted;
	}
}
