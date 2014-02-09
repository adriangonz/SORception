package com.sorception.jscrap.entities;

import java.util.List;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.OneToMany;
import javax.persistence.Table;

import com.fasterxml.jackson.annotation.JsonIgnore;

@Entity
@Table(name = "OrderLine")
public class OrderLineEntity extends AbstractEntity implements ISoftDeletable {
	@Column(name = "sgId", unique = true, nullable = false)
	private String sgId;
	
	@Column(name = "description")
	private String description;
	
	@Column(name = "quantity")
	private Integer quantity;
	
	@ManyToOne(fetch = FetchType.EAGER)
	@JoinColumn(name = "orderId", nullable = false)
	private OrderEntity order;
	
	@OneToMany(fetch = FetchType.EAGER, cascade = CascadeType.ALL,
			mappedBy = "orderLine")
	private List<OfferLineEntity> offerLines;
	
	@Column(name = "deleted", nullable = false)
	private Boolean deleted;
	
	public OrderLineEntity() {}
	
	public OrderLineEntity(String sgId,
			String description,
			Integer quantity) {
		this.sgId = sgId;
		this.description = description;
		this.quantity = quantity;
		this.deleted = false;
	}
	
	public String getSgId() {
		return sgId;
	}

	public String getDescription() {
		return description;
	}

	public Integer getQuantity() {
		return quantity;
	}
	
	public OfferLineEntity getOfferLine() {
		for(OfferLineEntity line : offerLines) {
			if(!line.isDeleted())
				return line;
		}
		return null;
	}
	
	@JsonIgnore
	public OrderEntity getOrder() {
		return order;
	}
	
	@JsonIgnore
	@Override
	public Boolean isDeleted() {
		return deleted;
	}
	
	public void setOrder(OrderEntity order) {
		this.order = order;
	}
	
	public void setOfferLine(OfferLineEntity offerLine) {
		for(OfferLineEntity line : offerLines) {
			line.setDeleted(true);
		}
		this.offerLines.add(offerLine);
	}

	@Override
	public void setDeleted(Boolean deleted) {
		this.deleted = deleted;
	}
	
	public void setQuantity(Integer quantity) {
		this.quantity = quantity;
	}
	
	public void setDescription(String description) {
		this.description = description;
	}
}
