package com.sorception.jscrap.services;

import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.dao.OfferDAO;
import com.sorception.jscrap.entities.OfferEntity;
import com.sorception.jscrap.entities.OfferLineEntity;
import com.sorception.jscrap.entities.OrderEntity;
import com.sorception.jscrap.entities.OrderLineEntity;
import com.sorception.jscrap.error.ResourceNotFoundException;

@Service
@Transactional
public class OfferService {
	@Autowired
	private OfferDAO offerDAO;
	
	@Autowired
	private OrderService orderService;
	
	final static Logger logger = LoggerFactory.getLogger(OfferService.class);
	
	public List<OfferEntity> getAllOffers() {
		return offerDAO.list();
	}
	
	public OfferEntity addOffer(List<OfferLineEntity> offerLines) {
		logger.info("Guardamos oferta con " 
				+ Integer.toString(offerLines.size()) + " lineas");
		OfferEntity offerEntity = new OfferEntity(offerLines);
		return offerDAO.save(offerEntity);
	}
	
	public OfferEntity getOfferById(Long id) {
		return offerDAO.get(id);
	}
}
