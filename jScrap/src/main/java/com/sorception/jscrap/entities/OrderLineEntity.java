package com.sorception.jscrap.entities;

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
	
	@OneToOne(fetch = FetchType.EAGER, cascade = CascadeType.ALL,
			mappedBy = "orderLine")
	private OfferLineEntity offerLine;
	
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
		return offerLine;
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
		this.offerLine = offerLine;
	}

	@Override
	public void setDeleted(Boolean deleted) {
		this.deleted = deleted;
	}
}
