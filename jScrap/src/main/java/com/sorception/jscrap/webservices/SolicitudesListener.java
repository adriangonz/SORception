/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.webservices;

import java.io.StringReader;
import java.util.ArrayList;
import java.util.List;

import com.sorception.jscrap.entities.OrderEntity;
import com.sorception.jscrap.entities.OrderLineEntity;
import com.sorception.jscrap.generated.AMQSolicitudMessage;
import com.sorception.jscrap.generated.ExposedLineaSolicitud;
import com.sorception.jscrap.generated.ExposedSolicitud;
import com.sorception.jscrap.generated.ObjectFactory;
import com.sorception.jscrap.services.OrderService;
import com.sorception.jscrap.services.TokenService;

import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.MessageListener;
import javax.jms.TextMessage;
import javax.xml.bind.JAXBElement;

import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.oxm.jaxb.Jaxb2Marshaller;
import org.springframework.xml.transform.StringSource;

/**
 *
 * @author kaseyo
 */
public class SolicitudesListener implements MessageListener {

    final static org.slf4j.Logger logger = LoggerFactory.getLogger(SolicitudesListener.class);
    
    @Autowired
    Jaxb2Marshaller unmarshaller;
    
    @Autowired
    ObjectFactory objectFactory;
    
    @Autowired
    OrderService orderService;
    
    public OrderEntity toOrderEntity(ExposedSolicitud solicitud) {    	
    	List<OrderLineEntity> lines = new ArrayList<>();    	
    	List<ExposedLineaSolicitud> lineasSolicitud = 
    			solicitud.getLineas().getValue().getExposedLineaSolicitud();
    	for(ExposedLineaSolicitud lineaSolicitud : lineasSolicitud) {
    		lines.add(new OrderLineEntity(lineaSolicitud.getId().toString(),
    				lineaSolicitud.getDescription().getValue(),
    				lineaSolicitud.getQuantity()));
    	}
    	return new OrderEntity(solicitud.getId().toString(), lines);
    }
    
    @Override
    public void onMessage(Message message) {
        try {
        	String xml = ((TextMessage)message).getText();
			// Desearilizing response
			JAXBElement<AMQSolicitudMessage> root = 
					(JAXBElement<AMQSolicitudMessage>) unmarshaller.unmarshal(new StringSource(xml));
			OrderEntity order = toOrderEntity(root.getValue().getSolicitud().getValue());
			logger.info("Saving new entity with SG-id " + order.getSgId());
			orderService.addOrder(order);
		} catch (JMSException e) {
			logger.error("'text' field not found at message");
		}
    }
    
}
