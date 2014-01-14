package com.sorception.jscrap.entities;

import java.util.List;

import javax.persistence.CascadeType;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.OneToMany;
import javax.persistence.Table;

import org.codehaus.jackson.annotate.JsonIgnore;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.services.ActiveMQService;

@Entity
@Table(name = "Offer")
public class OfferEntity extends AbstractEntity {
	
	final static Logger logger = LoggerFactory.getLogger(OfferEntity.class);
	
	@OneToMany(mappedBy = "_offer",
			cascade = CascadeType.ALL,
			fetch = FetchType.EAGER)
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
