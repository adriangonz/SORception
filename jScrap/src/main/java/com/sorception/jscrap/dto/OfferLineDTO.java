package com.sorception.jscrap.dto;

import java.util.Date;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

@JsonIgnoreProperties(ignoreUnknown = true)
public class OfferLineDTO {
	public Integer quantity;
	public String notes;
	public Long orderLineId;
	public Double price;
	public Date date;
	public Long id;
	public String status;
	
	public Boolean isDeleted() {
		return "DELETE".equals(status);
	}
}
