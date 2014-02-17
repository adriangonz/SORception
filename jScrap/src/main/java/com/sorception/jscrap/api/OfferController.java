package com.sorception.jscrap.api;

import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.ResponseStatus;

import com.sorception.jscrap.dto.OfferDTO;
import com.sorception.jscrap.entities.OfferEntity;
import com.sorception.jscrap.services.OfferService;

@Controller
@RequestMapping("/api/offer")
public class OfferController {
	final static Logger logger = LoggerFactory.getLogger(OfferController.class);
	
	@Autowired
	private OfferService offerService;
	
	@RequestMapping(value = "", method = RequestMethod.GET)
	@ResponseBody
	public List<OfferEntity> getOffers() {
		return offerService.getOpenedOffers();
	}
	
	@RequestMapping(value = "", method = RequestMethod.POST)
	@ResponseBody
	@ResponseStatus(HttpStatus.CREATED)
	public OfferEntity addOffer(@RequestBody OfferDTO offer) {
		return offerService.addOffer(offer);
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
		return offerService.updateOffer(offerId, offer);
	}
}
