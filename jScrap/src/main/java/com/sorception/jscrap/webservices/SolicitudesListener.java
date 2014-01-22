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
import com.sorception.jscrap.generated.ExpPedido;
import com.sorception.jscrap.generated.ExpSolicitud;
import com.sorception.jscrap.generated.ExpSolicitudLine;
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
import org.springframework.stereotype.Service;
import org.springframework.xml.transform.StringSource;

/**
 *
 * @author kaseyo
 */

@Service
public class SolicitudesListener implements MessageListener {

    final static org.slf4j.Logger logger = LoggerFactory.getLogger(SolicitudesListener.class);
    
    @Autowired
    Jaxb2Marshaller unmarshaller;
    
    @Autowired
    ObjectFactory objectFactory;
    
    @Autowired
    OrderService orderService;
    
    public OrderEntity toOrderEntity(ExpSolicitud solicitud) {    	
    	List<OrderLineEntity> lines = new ArrayList<>(); 	
    	List<ExpSolicitudLine> lineasSolicitud = 
    			solicitud.getLineas().getValue().getExpSolicitudLine();
    	for(ExpSolicitudLine lineaSolicitud : lineasSolicitud) {
    		lines.add(new OrderLineEntity(lineaSolicitud.getId().toString(),
    				lineaSolicitud.getDescription().getValue(),
    				lineaSolicitud.getQuantity()));
    	}
    	OrderEntity order = new OrderEntity(solicitud.getId().toString(), lines);
    	order.setDeadline(solicitud.getDeadline().toGregorianCalendar().getTime());
    	return order;
    }
    
    @Override
    public void onMessage(Message message) {
        try {
        	String xml = ((TextMessage)message).getText();
			// Deserializing response
			JAXBElement<AMQSolicitudMessage> root = 
					(JAXBElement<AMQSolicitudMessage>) unmarshaller.unmarshal(new StringSource(xml));
			AMQSolicitudMessage solicitudMessage = root.getValue();
			ExpSolicitud solicitud = solicitudMessage.getSolicitud().getValue();
			logger.info("Receiving order with remote id " 
					+ solicitud.getId() + " and code " + solicitudMessage.getCode());
			OrderEntity order;
			switch(solicitudMessage.getCode()) {
				case NEW:
					order = toOrderEntity(solicitud);
					orderService.addOrder(order);
					break;
				case CLOSED:
					order = orderService.getOrderBySgId(solicitud.getId().toString());
					orderService.closeOrder(order);
					break;
				case DELETE:
					order = orderService.getOrderBySgId(solicitud.getId().toString());
					orderService.deleteOrder(order);
			}
		} catch (JMSException e) {
			logger.error("'text' field not found at message");
		}
    }
    
}
