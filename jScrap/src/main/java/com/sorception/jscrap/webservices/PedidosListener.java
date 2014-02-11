package com.sorception.jscrap.webservices;

import java.util.ArrayList;

import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.MessageListener;
import javax.jms.TextMessage;
import javax.xml.bind.JAXBElement;

import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.oxm.jaxb.Jaxb2Marshaller;
import org.springframework.stereotype.Service;
import org.springframework.xml.transform.StringSource;

import com.sorception.jscrap.entities.AcceptedOfferLineEntity;
import com.sorception.jscrap.entities.OfferEntity;
import com.sorception.jscrap.entities.OfferLineEntity;
import com.sorception.jscrap.generated.AMQPedidoMessage;
import com.sorception.jscrap.generated.ExpPedido;
import com.sorception.jscrap.generated.ExpPedidoLine;
import com.sorception.jscrap.services.OfferService;
import com.sorception.jscrap.services.TokenService;

@Service
public class PedidosListener implements MessageListener {
	@Autowired
	Jaxb2Marshaller unmarshaller;
	
	@Autowired
	OfferService offerService;
	
	@Autowired
	TokenService tokenService;
	
	final static org.slf4j.Logger logger = LoggerFactory.getLogger(PedidosListener.class);
	
	public OfferEntity toOfferEntity(AMQPedidoMessage pedidoMessage) {
		ExpPedido pedido = pedidoMessage.getPedido().getValue();
		Long offerId = pedido.getOfertaId().longValue();
		OfferEntity offerEntity = offerService.getOfferById(offerId);
		ArrayList<OfferLineEntity> acceptedLines = new ArrayList<>();
		for(ExpPedidoLine linea : pedido.getLineas().getValue().getExpPedidoLine()) {
			OfferLineEntity offerLine = offerService.getOfferLine(linea.getLineaOfertaId().longValue());
			AcceptedOfferLineEntity acceptedOfferLine = new AcceptedOfferLineEntity();
			acceptedOfferLine.setQuantity(linea.getQuantity());
			offerLine.setAcceptedOffer(acceptedOfferLine);
			acceptedLines.add(offerLine);
		}
		offerEntity.setLines(acceptedLines);
		return offerEntity;
	}
	
	@Override
	public void onMessage(Message message) {
		String xml;
		try {
			xml = ((TextMessage)message).getText();
			// Deserializing response
			JAXBElement<AMQPedidoMessage> root = 
				(JAXBElement<AMQPedidoMessage>) unmarshaller.unmarshal(new StringSource(xml));
			AMQPedidoMessage pedidoMessage = root.getValue();
			if(pedidoMessage.getDesguaceId().getValue().equals(tokenService.getValid().getToken())) {
				logger.info("Saving accepted offer with id " + pedidoMessage.getPedido().getValue().getOfertaId());
				OfferEntity offer = toOfferEntity(pedidoMessage);
				offerService.updateOfferWithoutAMQ(offer);
			} else {
				logger.info("Ignoring accepted offer for another junkyard");
			}
		} catch (JMSException e) {
			logger.error("'text' field not found at message");
		}
	}
	
}
