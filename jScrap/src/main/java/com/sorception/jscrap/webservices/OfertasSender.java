package com.sorception.jscrap.webservices;

import java.io.StringWriter;

import javax.jms.Message;
import javax.xml.bind.JAXBElement;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jms.core.JmsTemplate;
import org.springframework.oxm.jaxb.Jaxb2Marshaller;
import org.springframework.stereotype.Service;
import org.springframework.xml.transform.StringResult;

import com.sorception.jscrap.entities.OfferEntity;
import com.sorception.jscrap.entities.OfferLineEntity;
import com.sorception.jscrap.entities.TokenEntity;
import com.sorception.jscrap.generated.AMQOfertaMessage;
import com.sorception.jscrap.generated.AMQOfertaMessageCode;
import com.sorception.jscrap.generated.ArrayOfExposedLineaOferta;
import com.sorception.jscrap.generated.ArrayOfExposedLineaSolicitud;
import com.sorception.jscrap.generated.ExposedLineaOferta;
import com.sorception.jscrap.generated.ExposedOferta;
import com.sorception.jscrap.generated.ObjectFactory;
import com.sorception.jscrap.services.OfferService;
import com.sorception.jscrap.services.OrderService;
import com.sorception.jscrap.services.TokenService;

@Service
public class OfertasSender {
	
	final static Logger logger = LoggerFactory.getLogger(OfertasSender.class);
	
	@Autowired
	JmsTemplate jmsTemplate;
	
	@Autowired
	ObjectFactory objectFactory;
	
	@Autowired
	Jaxb2Marshaller marshaller;
	
	@Autowired
	OfferService offerService;

	private ExposedLineaOferta toExposedLineaOferta(OfferLineEntity offerLine) {
		Integer idEnDesguace = offerLine.getId().intValue();
		Integer idLinea = Integer.parseInt(offerService.getOrderLine(offerLine).getSgId());
		JAXBElement<String> notes = 
				objectFactory.createExposedLineaOfertaNotes(offerLine.getNotes());
		Double price = offerLine.getPrice();
		Integer quantity = offerLine.getQuantity();
		
		ExposedLineaOferta exposedLineaOferta = 
				objectFactory.createExposedLineaOferta();
		exposedLineaOferta.setIdEnDesguace(idEnDesguace);
		exposedLineaOferta.setId(idLinea);
		exposedLineaOferta.setNotes(notes);
		exposedLineaOferta.setPrice(price);
		exposedLineaOferta.setQuantity(quantity);
		return exposedLineaOferta;
	}
	
	private ExposedOferta toExposedOferta(OfferEntity offerEntity, TokenEntity token) {
		JAXBElement<String> desguaceId = 
				objectFactory.createExposedOfertaDesguaceId(token.getToken());
		Integer id = offerEntity.getId().intValue();		
		Integer solicitudId = Integer.parseInt(offerEntity.getOrderSgId());
		ArrayOfExposedLineaOferta lineas = objectFactory.createArrayOfExposedLineaOferta();
		for(OfferLineEntity line : offerEntity.getLines()) {
			lineas.getExposedLineaOferta().add(toExposedLineaOferta(line));
		}
		JAXBElement<ArrayOfExposedLineaOferta> arrayOfLineas = 
				objectFactory.createExposedOfertaLineas(lineas);
		
		ExposedOferta exposedOferta = objectFactory.createExposedOferta();
		exposedOferta.setDesguaceId(desguaceId);
		exposedOferta.setId(id);
		exposedOferta.setSolicitudId(solicitudId);
		exposedOferta.setLineas(arrayOfLineas);
		return exposedOferta;
	}
	
	private AMQOfertaMessage toAMQOfertaMessage(
			OfferEntity offerEntity,TokenEntity token,
			AMQOfertaMessageCode code) {
		JAXBElement<ExposedOferta> oferta =
				objectFactory.createAMQOfertaMessageOferta((toExposedOferta(offerEntity, token)));
		
		AMQOfertaMessage amqOferta = objectFactory.createAMQOfertaMessage();
		amqOferta.setCode(code);
		amqOferta.setOferta(oferta);
		return amqOferta;
	}
	
	private String offerToString(
			OfferEntity offer, TokenEntity token, 
			AMQOfertaMessageCode code) {
		StringResult stringResult = new StringResult();
		marshaller.marshal(
				objectFactory.createAMQOfertaMessage(
						toAMQOfertaMessage(
								offer, token, AMQOfertaMessageCode.NEW)), 
				stringResult);
		return stringResult.toString();
	}
	
	public void sendNewOferta(OfferEntity offer, TokenEntity token) {
		logger.info("Sending a new offer to AMQ with local id " + offer.getId() + "...");
		jmsTemplate.convertAndSend(offerToString(offer, token, AMQOfertaMessageCode.NEW));
	}
	
	public void sendDeleteOferta(OfferEntity offer, TokenEntity token) {
		logger.info("Deleting offer from AMQ with local id " + offer.getId() + "...");
		jmsTemplate.convertAndSend(offerToString(offer, token, AMQOfertaMessageCode.DELETE));
	}
	
	public void sendUpdateOferta(OfferEntity offer, TokenEntity token) {
		logger.info("Updating offer from AMQ with local id " + offer.getId() + "...");
		jmsTemplate.convertAndSend(offerToString(offer, token, AMQOfertaMessageCode.UPDATE));
	}
}
