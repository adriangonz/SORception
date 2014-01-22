package com.sorception.jscrap.entities;

import java.util.ArrayList;
import java.util.Date;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.OneToMany;
import javax.persistence.Table;
import javax.persistence.Temporal;
import javax.persistence.TemporalType;

import com.fasterxml.jackson.annotation.JsonIgnore;

@Entity
@Table(name = "Orders")
public class OrderEntity extends AbstractEntity {
	@Column(name = "sgId")
	private String _sgId;
	
	@Column(name = "closed")
	private Boolean _closed = false;
	
	@OneToMany(mappedBy = "_order", 
			fetch = FetchType.EAGER,
			cascade = CascadeType.ALL)
	private List<OrderLineEntity> _lines;
	
	@Temporal(TemporalType.TIMESTAMP)
	@Column(name = "deadline")
    private Date _deadline;
	
	public OrderEntity() {}

	public OrderEntity(String sgId,
			List<OrderLineEntity> lines) {
		this._sgId = sgId;
		this._lines = lines;
		for(OrderLineEntity line : lines) {
			line.setOrder(this);
		}
	}
	
	public Date getDeadline() {
		return _deadline;
	}
	
	public String getSgId() {
		return _sgId;
	}

	@JsonIgnore
	public List<OrderLineEntity> getNotAccepted() {
		List<OrderLineEntity> lines = new ArrayList<>();
		for(OrderLineEntity line : _lines) {
			if(line.getOfferLine() == null || 
					(line.getOfferLine() != null && line.getOfferLine().getAcceptedOffer() == null)) {
				lines.add(line);
			}
		}
		return lines;
	}
	
	public List<OrderLineEntity> getLines() {
		return _lines;
	}
	
	public void setLines(List<OrderLineEntity> lines) {
		_lines = lines;
	}
	
	public Boolean isClosed() {
		return _closed != null ? _closed : false;
	}
	
	public void setDeadline(Date deadline) {
		this._deadline = deadline;
	}

	public void setClosed(boolean closed) {
		_closed = closed;
	}
	
	@JsonIgnore
	public OfferEntity getOffer() {
		if(!_lines.isEmpty() && _lines.get(0).getOfferLine() != null)
			return _lines.get(0).getOfferLine().getOffer();
		return null;
	}
}
