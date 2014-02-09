package com.sorception.jscrap.entities;

import java.util.ArrayList;
import java.util.List;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.OneToMany;
import javax.persistence.Table;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.fasterxml.jackson.annotation.JsonIgnore;

@Entity
@Table(name = "Offer")
public class OfferEntity extends AbstractEntity implements ISoftDeletable  {
	
	final static Logger logger = LoggerFactory.getLogger(OfferEntity.class);
	
	@OneToMany(mappedBy = "offer",
			cascade = CascadeType.ALL,
			fetch = FetchType.EAGER)
	private List<OfferLineEntity> lines;
	
	@Column(name = "deleted", nullable = false)
	private Boolean deleted = false;
		
	public OfferEntity() {}
	
	public OfferEntity(List<OfferLineEntity> lines) {
		this.setLines(lines);
	}
	
	public String getOrderSgId() {
		if(lines.size() > 0 && lines.get(0).getOrderLine() != null)
			return lines.get(0).getOrderLine().getOrder().getSgId();
		return null;
	}
	
	@JsonIgnore
	public OrderEntity getOrder() {
		for(OfferLineEntity offerline : lines) {
			if(!offerline.isDeleted()) //It is not deleted
				return offerline.getOrderLine().getOrder();
		}
		return null;
	}
	
	public Long getOrderId() {
		OrderEntity order = getOrder();
		return order != null ? order.getId() : null;
	}
	
	public List<OfferLineEntity> getLines() {
		List<OfferLineEntity> validLines = new ArrayList<>();
		for(OfferLineEntity line : lines) {
			if(!line.isDeleted())
				validLines.add(line);
		}
		return validLines;
	}
	
	@JsonIgnore
	public Boolean isDeleted() {
		return this.deleted;
	}
	
	@JsonIgnore
	public List<OfferLineEntity> getAccepted() {
		List<OfferLineEntity> accepted = new ArrayList<>();
		for(OfferLineEntity line : getLines()) {
			if(line.getAcceptedOffer() != null)
				accepted.add(line);
		}
		return accepted;
	}
	
	public void setLines(List<OfferLineEntity> lines) {
		this.lines = lines;
		for(OfferLineEntity line : lines) {
			line.setOffer(this);
		}
	}
	
	public void setDeleted(Boolean deleted) {
		this.deleted = deleted;
	}
}
