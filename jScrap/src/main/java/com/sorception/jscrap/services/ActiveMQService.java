package com.sorception.jscrap.services;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jms.listener.DefaultMessageListenerContainer;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.config.ActiveMQConfig;
import com.sorception.jscrap.entities.OfferEntity;
import com.sorception.jscrap.entities.TokenEntity;
import com.sorception.jscrap.webservices.OfertasSender;

@Service
@Transactional
public class ActiveMQService {
	
	final static Logger logger = LoggerFactory.getLogger(ActiveMQService.class);
	
    @Autowired
    DefaultMessageListenerContainer pedidosContainer;
    
    @Autowired
    DefaultMessageListenerContainer solicitudesContainer;
    
    @Autowired
    OfertasSender ofertasSender;
    
    @Autowired
    ActiveMQConfig amqConfig;
	
    public void enableJmsContainers(TokenEntity tokenEntity) {
    	logger.info("Enabling JMS containers.");
    	amqConfig.enableJmsContainer(pedidosContainer, tokenEntity, "Pedidos");
    	amqConfig.enableJmsContainer(solicitudesContainer, tokenEntity, "Solicitudes");
    }
    
    public void disableJmsContainers() {
    	logger.info("Disabling JMS containers.");
    	amqConfig.disableJmsContainer(pedidosContainer);
    	amqConfig.disableJmsContainer(solicitudesContainer);
    }
    
    public void sendNewOffer(OfferEntity offer, TokenEntity token) {
		logger.info("Sending a new offer to AMQ with local id " + offer.getId() + ".");
    	ofertasSender.sendNewOferta(offer, token);
    }
    
    public void sendDeleteOffer(OfferEntity offer, TokenEntity token) {
    	logger.info("Deleting offer from AMQ with local id " + offer.getId() + ".");
    	ofertasSender.sendDeleteOferta(offer, token);
    }
    
    public void sendUpdateOffer(OfferEntity offer, TokenEntity token) {
    	logger.info("Updating offer from AMQ with local id " + offer.getId() + ".");
    	ofertasSender.sendUpdateOferta(offer, token);
    }
}
