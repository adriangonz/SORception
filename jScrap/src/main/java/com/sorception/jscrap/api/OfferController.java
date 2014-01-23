package com.sorception.jscrap.api;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.ApplicationContext;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Controller;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.ResponseStatus;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.sorception.jscrap.entities.OfferEntity;
import com.sorception.jscrap.entities.OfferLineEntity;
import com.sorception.jscrap.services.OfferService;
import com.sorception.jscrap.services.OrderService;

@JsonIgnoreProperties(ignoreUnknown = true)
class OfferLineDTO {
	public Integer quantity;
	public String notes;
	public Long orderLineId;
	public Double price;
	public Date date;
	public Long id;
	public String status;
	
	public OfferLineEntity getOfferLine(
			OrderService orderService,
			OfferService offerService) {
		OfferLineEntity offerLine;
		if(id == null) {
			offerLine = new OfferLineEntity(
					quantity,
					notes,
					price,
					date,
					orderService.getOrderLine(orderLineId)
			);
		} else {
			offerLine = offerService.getOfferLine(id);
			if(quantity != null)
				offerLine.setQuantity(quantity);
			if(notes != null)
				offerLine.setNotes(notes);
			if(price != null)
				offerLine.setPrice(price);
			if(date != null)
				offerLine.setDate(date);
			if(status != null && "DELETE".equals(status))
				offerLine.delete();
		}
		return offerLine;
	}
}

@JsonIgnoreProperties(ignoreUnknown = true)
class OfferDTO {
	final static Logger logger = LoggerFactory.getLogger(OfferController.class);
	
	public List<OfferLineDTO> lines;
			
	public List<OfferLineEntity> getOfferLines(
			OrderService orderService,
			OfferService offerService) {
		List<OfferLineEntity> list = new ArrayList<>();
		for(OfferLineDTO line : this.lines) {
			list.add(line.getOfferLine(orderService, offerService));
		}
		return list;
	}
}

@Controller
@RequestMapping("/api/offer")
public class OfferController {
	final static Logger logger = LoggerFactory.getLogger(OfferController.class);
	
	@Autowired
	private OfferService offerService;
	
	@Autowired
	private OrderService orderService;
	
	@RequestMapping(value = "", method = RequestMethod.GET)
	@ResponseBody
	public List<OfferEntity> getOffers() {
		return offerService.getAllOffers();
	}
	
	@RequestMapping(value = "", method = RequestMethod.POST)
	@ResponseBody
	@ResponseStatus(HttpStatus.CREATED)
	public OfferEntity addOffer(@RequestBody OfferDTO offer) {
		return offerService.addOffer(offer.getOfferLines(
				orderService,
				offerService));
	}
	
	@RequestMapping(value = "/{offerId}", method = RequestMethod.GET)
	@ResponseBody
	public OfferEntity get(@PathVariable Long offerId) {
		return offerService.getOfferById(offerId);
	}
	
	@RequestMapping(value = "/{offerId}", method = RequestMethod.DELETE)
	@ResponseStatus(HttpStatus.OK)
	public void deleteOffer(@PathVariable Long offerId) {
		offerService.deleteOffer(offerId);
	}
	
	@RequestMapping(value = "/{offerId}", method = RequestMethod.PUT)
	@ResponseBody
	@ResponseStatus(HttpStatus.OK)
	public OfferEntity updateOffer(
			@PathVariable Long offerId, 
			@RequestBody OfferDTO offer) {
		return offerService.updateOffer(offerId, offer.getOfferLines(
				orderService,
				offerService));
	}
}
