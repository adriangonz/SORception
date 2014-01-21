package com.sorception.jscrap.entities;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.JoinColumn;
import javax.persistence.OneToOne;
import javax.persistence.Table;

@Entity
@Table(name = "AcceptedOfferLine")
public class AcceptedOfferLineEntity extends AbstractEntity {
	
	@Column(name = "quantity")
	private int _quantity;
	
	@OneToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "offerLineId", unique = true)
	private OfferLineEntity _offerLine;
	
	public int getQuantity() {
		return _quantity;
	}
	
	public void setQuantity(int quantity) {
		_quantity = quantity;
	}
	
	public void setOfferLine(OfferLineEntity offerLine) {
		_offerLine = offerLine;
	}
}
