/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.config;

import com.sorception.jscrap.activemq.SolicitudesListener;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.jms.Connection;
import javax.jms.ConnectionFactory;
import javax.jms.JMSException;
import org.apache.activemq.ActiveMQConnectionFactory;
import org.apache.activemq.broker.BrokerService;
import org.apache.activemq.broker.TransportConnector;
import org.apache.activemq.command.ActiveMQTopic;
import org.apache.activemq.pool.PooledConnectionFactoryBean;
import org.apache.activemq.xbean.BrokerFactoryBean;
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
    public ConnectionFactory jmsFactory() {
        return new ActiveMQConnectionFactory(_activemqUrl);
    }
    
    @Bean
    public JmsTemplate jmsTemplate() {
        return new JmsTemplate(jmsFactory());
    }

    @Bean
    public SolicitudesListener messageListener() {
        return new SolicitudesListener();
    }

    @Bean
    public DefaultMessageListenerContainer jmsContainer() {
        DefaultMessageListenerContainer jmsContainer = new DefaultMessageListenerContainer();
        jmsContainer.setConnectionFactory(jmsFactory());
        jmsContainer.setMessageListener(messageListener());
        jmsContainer.setDestination(solicitudes());
        return jmsContainer;
    }
}
