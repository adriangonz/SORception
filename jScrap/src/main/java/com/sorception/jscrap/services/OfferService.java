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
		
	public List<OfferEntity> getAllOffers() {
		return getOfferDao().getOpenedOffers();
	}
	
	public List<OfferEntity> getAcceptedOffers() {
		return getOfferDao().getAcceptedOffers();
	}
	
	public OfferEntity addOffer(OfferDTO offerDTO) {
		if(offerDTO.lines.size() == 0)
			throw new BusinessException("Cannot create offer with no lines");
		OfferEntity offerEntity = toOfferEntity(offerDTO);
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
	
	public OfferEntity updateOffer(Long offerId, OfferDTO offerToUpdate) {
		OfferEntity offer = toOfferEntity(offerId, offerToUpdate);
		amqService.sendUpdateOffer(offer, tokenService.getValid());
		return offer;
	}
	
	public OfferLineEntity getOfferLine(Long id) {
		OfferLineEntity line = getOfferDao().getOfferlineById(id);
		if(line == null)
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
		for(OfferLineDTO lineDTO : offerToUpdate.lines) {
			if(lineDTO.id == null) 
				lines.add(toOfferLineEntity(lineDTO));
			else {
				OfferLineEntity line = getOfferLine(lineDTO.id);
				if(lineDTO.isDeleted()) {
					line.setDeleted(true);
					line.setOrderLine(null);
				} else {
					line.setDate(lineDTO.date);
					line.setNotes(lineDTO.notes);
					line.setPrice(lineDTO.price);
					line.setQuantity(lineDTO.quantity);
				}
				lines.add(line);
			}
		}
		offer.setLines(lines);
		return offer;
	}
}
