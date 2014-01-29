package com.sorception.jscrap.services;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.dao.IGenericDAO;
import com.sorception.jscrap.dao.IOfferDAO;
import com.sorception.jscrap.dao.IOfferLineDAO;
import com.sorception.jscrap.entities.OfferEntity;
import com.sorception.jscrap.entities.OfferLineEntity;

@Service
@Transactional
public class OfferService extends AbstractService<OfferEntity> {
	
	public OfferService() {
		super(OfferEntity.class);
	}

	@Override
	protected IGenericDAO<OfferEntity> getDao() {
		return dao;
	}
	
	private IOfferDAO getOfferDao() {
		return dao;
	}
	
	private IOfferLineDAO getOfferLineDao() {
		return offerLineDao;
	}
	
	@Autowired 
	private IOfferDAO dao;
	
	@Autowired
	private IOfferLineDAO offerLineDao;
	
	@Autowired
	private OrderService orderService;
	
	@Autowired
	private ActiveMQService amqService;
	
	@Autowired
	private TokenService tokenService;
		
	public List<OfferEntity> getAllOffers() {
		return getOfferDao().getOpenedOffers();
	}
	
	public List<OfferEntity> getAcceptedOffers() {
		return getOfferDao().getAcceptedOffers();
	}
	
	public OfferEntity addOffer(List<OfferLineEntity> offerLines) {
		OfferEntity offerEntity = new OfferEntity(offerLines);
		offerEntity = create(offerEntity);
		amqService.sendNewOffer(offerEntity, tokenService.getValid());
		return offerEntity;
	}
	
	public OfferEntity getOfferById(Long id) {
		return this.findOne(id);
	}
	
	public void deleteOffer(Long id) {
		OfferEntity offer = this.findOne(id);
		deleteOffer(offer);
	}
	
	public void deleteOffer(OfferEntity offer) {
		amqService.sendDeleteOffer(offer,  tokenService.getValid());
		deleteOfferWithoutAMQ(offer);
	}
	
	public void deleteOfferWithoutAMQ(OfferEntity offer) {
		this.delete(offer);
	}
	
	public OfferEntity updateOffer(Long offerId, List<OfferLineEntity> lines) {
		// Check if we are going to erase all lines (i.e. erase offer)
		OfferEntity offer = updateOfferWithoutAMQ(offerId, lines);
		amqService.sendUpdateOffer(offer, tokenService.getValid());
		return offer;
	}
	
	public OfferEntity updateOfferWithoutAMQ(Long offerId, List<OfferLineEntity> lines) {
		OfferEntity offer = this.getOfferById(offerId);
		offer.setLines(lines);
		this.update(offer);
		offer = this.getOfferById(offer.getId());
		return offer;
	}
	
	public OfferLineEntity getOfferLine(Long id) {
		return getOfferLineDao().findOne(id);
	}
}
