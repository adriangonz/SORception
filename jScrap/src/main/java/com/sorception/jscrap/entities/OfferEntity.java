package com.sorception.jscrap.entities;

import java.util.List;

import javax.persistence.CascadeType;
import javax.persistence.Entity;
import javax.persistence.OneToMany;
import javax.persistence.Table;

@Entity
@Table(name = "Offer")
public class OfferEntity extends AbstractEntity {
	@OneToMany(mappedBy = "_offer",
			cascade = CascadeType.ALL)
	private List<OfferLineEntity> _lines;
	
	public OfferEntity() {}
	
	public OfferEntity(List<OfferLineEntity> lines) {
		this._lines = lines;
		for(OfferLineEntity line : lines) {
			line.setOffer(this);
		}
	}
	
	public List<OfferLineEntity> getLines() {
		return this._lines;
	}
}
