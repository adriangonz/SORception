/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.config;

import com.sorception.jscrap.activemq.SolicitudesListener;
import com.sorception.jscrap.entities.TokenEntity;
import com.sorception.jscrap.error.ResourceNotFoundException;
import com.sorception.jscrap.services.TokenService;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.jms.Connection;
import javax.jms.ConnectionFactory;
import javax.jms.JMSException;
import org.apache.activemq.ActiveMQConnectionFactory;
import org.apache.activemq.ActiveMQXAConnectionFactory;
import org.apache.activemq.broker.BrokerService;
import org.apache.activemq.broker.TransportConnector;
import org.apache.activemq.command.ActiveMQTopic;
import org.apache.activemq.pool.PooledConnectionFactoryBean;
import org.apache.activemq.xbean.BrokerFactoryBean;
import org.hibernate.engine.transaction.internal.jta.JtaTransactionFactory;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.jms.core.JmsTemplate;
import org.springframework.jms.listener.DefaultMessageListenerContainer;

/**
 *
 * @author kaseyo
 */
@Configuration
public class ActiveMQConfig {
    final static org.slf4j.Logger logger = LoggerFactory.getLogger(ActiveMQConfig.class);
    
    @Autowired
    TokenService tokenService;
    
    @Value("${activemq.url}")
    private String _activemqUrl;
    
    @Bean
    public ActiveMQTopic solicitudes() {
        return new ActiveMQTopic("Solicitudes");
    }
    
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
        return jmsTemplate;
    }

    @Bean
    public SolicitudesListener messageListener() {
        return new SolicitudesListener();
    }
    
    public void enableJmsContainer(DefaultMessageListenerContainer jmsContainer, TokenEntity validToken) {
    	logger.info("Valid token found! - Starting jmsContainer with token " + validToken.getToken() + 
    			" and subscribing to topic " + jmsContainer.getDestinationName() + "...");
        jmsContainer.setDurableSubscriptionName(validToken.getToken());
        jmsContainer.setClientId(validToken.getToken());
        jmsContainer.setSubscriptionDurable(true);
        jmsContainer.start();
        jmsContainer.setAutoStartup(true);
    }
    
    public void disableJmsContainer(DefaultMessageListenerContainer jmsContainer) {
    	logger.info("Not valid token found - Disabling jmsContainer...");
    	jmsContainer.stop();
        jmsContainer.setAutoStartup(false);
    }

    @Bean
    public DefaultMessageListenerContainer jmsContainer() {
        DefaultMessageListenerContainer jmsContainer = new DefaultMessageListenerContainer();
        jmsContainer.setConnectionFactory(connectionFactory());
        jmsContainer.setMessageListener(messageListener());
        jmsContainer.setDestination(solicitudes());
        jmsContainer.setPubSubDomain(true);
        try {
            TokenEntity validToken = tokenService.getValid();
            enableJmsContainer(jmsContainer, validToken);
        } catch(ResourceNotFoundException ex) {
            disableJmsContainer(jmsContainer);
        }
        return jmsContainer;
    }
}
