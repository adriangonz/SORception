package com.sorception.jscrap.api;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;

import com.sorception.jscrap.entities.OfferEntity;
import com.sorception.jscrap.services.OfferService;

@Controller
@RequestMapping("/api/accepted")
public class AcceptedController {
	@Autowired
	private OfferService offerService;
	
	@RequestMapping(value = "", method = RequestMethod.GET)
	@ResponseBody
	public List<OfferEntity> getOffers() {
		return offerService.getAcceptedOffers();
	}
}
