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
    DefaultMessageListenerContainer jmsContainer;
    
    @Autowired
    OfertasSender ofertasSender;
    
    @Autowired
    ActiveMQConfig amqConfig;
	
    public void enableJmsContainer(TokenEntity tokenEntity) {
    	amqConfig.enableJmsContainer(jmsContainer, tokenEntity);
    }
    
    public void disableJmsContainer() {
    	amqConfig.disableJmsContainer(jmsContainer);
    }
    
    public void sendNewOffer(OfferEntity offer, TokenEntity token) {
    	ofertasSender.sendNewOferta(offer, token);
    }
    
    public void sendDeleteOffer(OfferEntity offer, TokenEntity token) {
    	ofertasSender.sendDeleteOferta(offer, token);
    }
    
    public void sendUpdateOffer(OfferEntity offer, TokenEntity token) {
    	ofertasSender.sendUpdateOferta(offer, token);
    }
}
