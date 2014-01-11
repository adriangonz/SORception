package com.sorception.jscrap.webservices;

import javax.jms.Message;
import javax.xml.bind.JAXBElement;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jms.core.JmsTemplate;
import org.springframework.oxm.jaxb.Jaxb2Marshaller;
import org.springframework.stereotype.Service;

import com.sorception.jscrap.entities.OfferEntity;
import com.sorception.jscrap.entities.OfferLineEntity;
import com.sorception.jscrap.entities.TokenEntity;
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
	
	final static Logger logger = LoggerFactory.getLogger(OfferEntity.class);
	
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
		exposedLineaOferta.setIdLinea(idLinea);
		exposedLineaOferta.setNotes(notes);
		exposedLineaOferta.setPrice(price);
		exposedLineaOferta.setQuantity(quantity);
		return exposedLineaOferta;
	}
	
	private ExposedOferta toExposedOferta(OfferEntity offerEntity, TokenEntity token) {
		Integer desguaceId = 0;//token.getToken();
		Integer id = offerEntity.getId().intValue();
		Integer solicitudId = Integer.parseInt(offerService.getOrder(offerEntity).getSgId());
		ArrayOfExposedLineaOferta lineas = objectFactory.createArrayOfExposedLineaOferta();
		for(OfferLineEntity line : offerEntity.getLines()) {
			lineas.getExposedLineaOferta().add(toExposedLineaOferta(line));
		}
		JAXBElement<ArrayOfExposedLineaOferta> arrayOfLineas = 
				objectFactory.createArrayOfExposedLineaOferta(lineas);
		
		ExposedOferta exposedOferta = objectFactory.createExposedOferta();
		exposedOferta.setDesguaceId(desguaceId);
		exposedOferta.setId(id);
		exposedOferta.setSolicitudId(solicitudId);
		exposedOferta.setLineas(arrayOfLineas);
		return exposedOferta;
	}
	
	public void sendOferta(OfferEntity offerEntity, TokenEntity token) {
		jmsTemplate.convertAndSend(
				objectFactory.createExposedOferta(toExposedOferta(offerEntity, token)));
	}
}
