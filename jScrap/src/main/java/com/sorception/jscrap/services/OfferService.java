package com.sorception.jscrap.services;

import java.util.ArrayList;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.dao.IGenericDAO;
import com.sorception.jscrap.dao.IOfferDAO;
import com.sorception.jscrap.dto.OfferDTO;
import com.sorception.jscrap.dto.OfferLineDTO;
import com.sorception.jscrap.entities.OfferEntity;
import com.sorception.jscrap.entities.OfferLineEntity;
import com.sorception.jscrap.error.BusinessException;
import com.sorception.jscrap.error.ResourceNotFoundException;

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
	
	@Autowired 
	private IOfferDAO dao;
	
	@Autowired
	private OrderService orderService;
	
	@Autowired
	private ActiveMQService amqService;
	
	@Autowired
	private TokenService tokenService;
		
	public List<OfferEntity> getOpenedOffers() {
		logger.info("Retrieving all opened offers.");
		return getOfferDao().getOpenedOffers();
	}
	
	public List<OfferEntity> getAcceptedOffers() {
		logger.info("Retrieving all accepted offers.");
		return getOfferDao().getAcceptedOffers();
	}
	
	public OfferEntity addOffer(OfferDTO offerDTO) {
		logger.info("Adding new offer.");
		if(offerDTO.lines.size() == 0)
			throw new BusinessException("Cannot create offer with no lines.");
		OfferEntity offerEntity = toOfferEntity(offerDTO);
		offerEntity = create(offerEntity);
		amqService.sendNewOffer(offerEntity, tokenService.getValid());
		return offerEntity;
	}
	
	public OfferEntity getOfferById(Long id) {
		logger.info("Retrieving offer with id " + id + ".");
		return this.findOne(id);
	}
	
	public void deleteOffer(Long id) {
		OfferEntity offer = this.getOfferById(id);
		deleteOffer(offer);
	}
	
	public void deleteOffer(OfferEntity offer) {
		logger.info("Deleting offer with id " + offer.getId() + ".");
		amqService.sendDeleteOffer(offer,  tokenService.getValid());
		deleteOfferWithoutAMQ(offer);
	}
	
	public void deleteOfferWithoutAMQ(OfferEntity offer) {
		this.delete(offer);
	}
	
	public OfferEntity updateOffer(Long offerId, OfferDTO offerToUpdate) {
		logger.info("Updating offer with id " + offerId + ".");
		OfferEntity offer = toOfferEntity(offerId, offerToUpdate);
		update(offer);
		if(!offer.isDeleted()) {
			amqService.sendUpdateOffer(offer, tokenService.getValid());
			return offer;
		} else {
			deleteOffer(offer);
			return null;
		}
	}
	
	public OfferEntity updateOfferWithoutAMQ(OfferEntity offer) {
		update(offer);
		return offer;
	}
	
	public OfferLineEntity getOfferLine(Long id) {
		logger.info("Retrieving offerline with id " + id + ".");
		OfferLineEntity line = getOfferDao().getOfferlineById(id);
		if(line == null || line.isDeleted())
			throw new ResourceNotFoundException("Offerline with id " + Long.toString(id) + " was not found");
		return line;
	}
	
	private OfferEntity toOfferEntity(OfferDTO offer) {
		List<OfferLineEntity> lines = new ArrayList<>();
		for(OfferLineDTO lineDTO : offer.lines) {
			lines.add(toOfferLineEntity(lineDTO));
		}
		return new OfferEntity(lines);
	}
	
	private OfferLineEntity toOfferLineEntity(OfferLineDTO lineDTO) {
		return new OfferLineEntity(
				lineDTO.quantity,
				lineDTO.notes,
				lineDTO.price,
				lineDTO.date,
				orderService.getOrderLine(lineDTO.orderLineId)
			);
	}
	
	private OfferEntity toOfferEntity(Long offerId, OfferDTO offerToUpdate) {
		OfferEntity offer = getOfferById(offerId);
		List<OfferLineEntity> lines = new ArrayList<>();
		Boolean removeOffer = true;
		for(OfferLineDTO lineDTO : offerToUpdate.lines) {
			if(lineDTO.id == null) {
				lines.add(toOfferLineEntity(lineDTO));
				removeOffer = false;
			} else {
				OfferLineEntity line = getOfferLine(lineDTO.id);
				if(lineDTO.isDeleted()) {
					line.setDeleted(true);
				} else {
					line.setDate(lineDTO.date);
					line.setNotes(lineDTO.notes);
					line.setPrice(lineDTO.price);
					line.setQuantity(lineDTO.quantity);
					removeOffer = false;
				}
				lines.add(line);
			}
		}
		if(removeOffer)
			offer.setDeleted(true);
		offer.setLines(lines);
		return offer;
	}
}
