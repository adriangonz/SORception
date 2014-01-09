package com.sorception.jscrap.entities;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.OneToOne;
import javax.persistence.Table;

@Entity
@Table(name = "OfferLine")
public class OfferLineEntity extends AbstractEntity {
	
	@Column(name = "quantity")
	private Integer _quantity;
	
	@Column(name = "notes")
	private String _notes;
	
	@ManyToOne(fetch = FetchType.EAGER)
	@JoinColumn(name = "offerId", nullable = false)
	private OfferEntity _offer;
	
	@OneToOne(fetch = FetchType.LAZY, optional = false)
	@JoinColumn(name="orderLineId")
	private OrderLineEntity _orderLine;
	
	public OfferLineEntity() {}
	
	public OfferLineEntity(Integer quantity,
			String notes,
			OrderLineEntity orderLine) {
		this._quantity = quantity;
		this._notes = notes;
		this._orderLine = orderLine;
		this._orderLine.setOfferLine(this);
	}
	
	public void setOffer(OfferEntity offer) {
		this._offer = offer;
	}

	public Integer getQuantity() {
		return _quantity;
	}

	public String getNotes() {
		return _notes;
	}
}
