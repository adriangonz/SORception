/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.config;


import javax.jms.ConnectionFactory;
import javax.jms.Destination;
import javax.jms.MessageListener;

import org.apache.activemq.ActiveMQConnectionFactory;
import org.apache.activemq.command.ActiveMQTopic;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.jms.core.JmsTemplate;
import org.springframework.jms.listener.DefaultMessageListenerContainer;
import org.springframework.oxm.jaxb.Jaxb2Marshaller;

import com.sorception.jscrap.entities.TokenEntity;
import com.sorception.jscrap.error.ResourceNotFoundException;
import com.sorception.jscrap.services.SettingsService;
import com.sorception.jscrap.services.TokenService;

/**
 *
 * @author kaseyo
 */
@Configuration
public class ActiveMQConfig {
    final static org.slf4j.Logger logger = LoggerFactory.getLogger(ActiveMQConfig.class);
    
    @Autowired
    TokenService tokenService;
    
    @Autowired
    Jaxb2Marshaller marshaller;
    
    @Autowired
    Jaxb2Marshaller unmarshaller;
    
    @Autowired
    MessageListener solicitudesListener;
    
    @Autowired
    MessageListener pedidosListener;
    
    @Value("${activemq.url}")
    private String _activemqUrl;
    
    @Autowired
    SettingsService settingsService;
    
    @Bean
    public ActiveMQTopic ofertas() {
        return new ActiveMQTopic("Ofertas");
    }
    
    @Bean
    public ConnectionFactory connectionFactory() {
         ActiveMQConnectionFactory cnf = new ActiveMQConnectionFactory();
         cnf.setBrokerURL(_activemqUrl);
         return cnf;
    }
    
    @Bean
    public JmsTemplate jmsTemplate() {
        JmsTemplate jmsTemplate = new JmsTemplate(connectionFactory());
        jmsTemplate.setPubSubDomain(true);
        jmsTemplate.setDefaultDestination(ofertas());
        jmsTemplate.setConnectionFactory(connectionFactory());
        //MarshallingMessageConverter converter = new MarshallingMessageConverter(marshaller, unmarshaller);
        //jmsTemplate.setMessageConverter(converter);
        return jmsTemplate;
    }
    
    public void enableJmsContainer(DefaultMessageListenerContainer jmsContainer, TokenEntity validToken, String destination) {
    	logger.info("Valid token found! - Starting jmsContainer with token " + validToken.getToken() + 
    			" and subscribing to topic " + destination + "...");
        jmsContainer.setDurableSubscriptionName(settingsService.getGlobalSettings().getName());
        jmsContainer.setClientId(validToken.getToken() + "@" + destination);
        jmsContainer.setSubscriptionDurable(true);
        jmsContainer.setPubSubDomain(true);
        jmsContainer.start();
        jmsContainer.setAutoStartup(true);
    }
    
    public void disableJmsContainer(DefaultMessageListenerContainer jmsContainer) {
    	logger.info("Not valid token found - Disabling jmsContainer listening on " + jmsContainer.getDestinationName() + "...");
    	jmsContainer.stop();
        jmsContainer.setAutoStartup(false);
    }
    
    private DefaultMessageListenerContainer createContainer(String destinationName, MessageListener listener) {
    	Destination destination = new ActiveMQTopic(destinationName);
    	DefaultMessageListenerContainer jmsContainer = new DefaultMessageListenerContainer();
        jmsContainer.setConnectionFactory(connectionFactory());
        jmsContainer.setMessageListener(listener);
        jmsContainer.setDestination(destination);
        try {
            TokenEntity validToken = tokenService.getValidInit();
            enableJmsContainer(jmsContainer, validToken, destinationName);
        } catch(ResourceNotFoundException ex) {
            disableJmsContainer(jmsContainer);
        }
        return jmsContainer;
    }

    @Bean
    public DefaultMessageListenerContainer solicitudesContainer() {
    	return createContainer("Solicitudes", solicitudesListener);
    }
    
    @Bean
    public DefaultMessageListenerContainer pedidosContainer() {
    	return createContainer("Pedidos", pedidosListener);
    }
}
