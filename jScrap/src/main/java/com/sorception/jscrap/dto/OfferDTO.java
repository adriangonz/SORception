package com.sorception.jscrap.dto;

import java.util.List;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

@JsonIgnoreProperties(ignoreUnknown = true)
public class OfferDTO {	
	public List<OfferLineDTO> lines;
}
