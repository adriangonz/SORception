package com.sorception.jscrap.api;

import java.util.ArrayList;
import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Controller;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.ResponseStatus;

import com.sorception.jscrap.entities.OfferEntity;
import com.sorception.jscrap.entities.OfferLineEntity;
import com.sorception.jscrap.services.OfferService;
import com.sorception.jscrap.services.OrderService;

class OfferLineDTO {
	public Integer quantity;
	public String notes;
	public Long orderLineId;
	final static Logger logger = LoggerFactory.getLogger(OfferController.class);
	public OfferLineEntity getOfferLine(OrderService orderService) {
		return new OfferLineEntity(
				quantity,
				notes,
				orderService.getOrderLine(orderLineId)
		);
	}
}

class OfferDTO {
	public List<OfferLineDTO> lines;
			
	public List<OfferLineEntity> getOfferLines(OrderService orderService) {
		List<OfferLineEntity> list = new ArrayList<>();
		for(OfferLineDTO line : this.lines) {
			list.add(line.getOfferLine(orderService));
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
		return offerService.addOffer(offer.getOfferLines(orderService));
	}
}
