package com.sorception.jscrap.services;

import java.util.Iterator;
import java.util.List;

import org.hibernate.Hibernate;
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
import com.sorception.jscrap.webservices.OfertasSender;

@Service
@Transactional
public class OfferService {
	@Autowired
	private OfferDAO offerDAO;
	
	@Autowired
	private OrderService orderService;
	
	@Autowired
	private ActiveMQService amqService;
	
	@Autowired
	private TokenService tokenService;
	
	final static Logger logger = LoggerFactory.getLogger(OfferService.class);
	
	public List<OfferEntity> getAllOffers() {
		return offerDAO.list();
	}
	
	public OfferEntity addOffer(List<OfferLineEntity> offerLines) {
		OfferEntity offerEntity = new OfferEntity(offerLines);
		offerDAO.save(offerEntity);
		amqService.sendNewOffer(offerEntity, tokenService.getValid());
		return offerEntity;
	}
	
	public OfferEntity getOfferById(Long id) {
		OfferEntity offer = offerDAO.get(id);
		if(offer == null)
			throw new ResourceNotFoundException("Offer with id " + id + " was not found");
		return offer;
	}
	
	public void deleteOffer(Long id) {
		OfferEntity offer = offerDAO.get(id);
		offerDAO.delete(offer);
		amqService.sendDeleteOffer(offer,  tokenService.getValid());
	}
	
	public OfferEntity updateOffer(Long offerId, List<OfferLineEntity> lines) {
		// Check if we are going to erase all lines (i.e. erase offer)
		OfferEntity offer = this.getOfferById(offerId);
		Integer totalToDelete = 0;
		for(OfferLineEntity line : lines) {
			if(line.toDelete())
				totalToDelete++;
		}
		if(offer.getLines().size() == totalToDelete) {
			this.deleteOffer(offerId);
			return null;
		} else {
			// Remove entities marked to delete
			for(Iterator<OfferLineEntity> itLine = lines.iterator(); itLine.hasNext();) {
			   OfferLineEntity line = itLine.next();
			   if(line.toDelete()) {
				   offerDAO.delete(line);
				   itLine.remove(); // Remove from list   
			   }
			}
			offer.setLines(lines);
			OfferEntity new_offer = offerDAO.update(offer);
			amqService.sendUpdateOffer(new_offer, tokenService.getValid());
			return new_offer;
		}
	}
	
	public OfferLineEntity getOfferLine(Long id) {
		return offerDAO.getOfferLine(id);
	}
	
	/* START OF NYAPICA */
	public OrderLineEntity getOrderLine(OfferLineEntity offerLine) {
		return offerDAO.getOrderLine(offerLine);
	}
	
	public OrderEntity getOrder(OfferEntity offer) {
		return offerDAO.getOrder(offer);
	}
	
	/* END OF NYAPICA */
}
