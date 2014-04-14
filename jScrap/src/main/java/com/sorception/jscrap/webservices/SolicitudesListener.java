/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.webservices;

import java.util.ArrayList;
import java.util.List;

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

import com.sorception.jscrap.entities.OrderEntity;
import com.sorception.jscrap.entities.OrderLineEntity;
import com.sorception.jscrap.generated.AMQSecureMessage;
import com.sorception.jscrap.generated.AMQSolicitudMessage;
import com.sorception.jscrap.generated.ExpSolicitud;
import com.sorception.jscrap.generated.ExpSolicitudLine;
import com.sorception.jscrap.generated.ObjectFactory;
import com.sorception.jscrap.services.CryptoService;
import com.sorception.jscrap.services.OrderService;

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
    CryptoService cryptoService;
    
    @Autowired
    ObjectFactory objectFactory;
    
    @Autowired
    OrderService orderService;
    
    private OrderEntity toOrderEntity(ExpSolicitud solicitud) {    	
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
    
    private OrderEntity getOrderToUpdate(OrderEntity order) {
    	OrderEntity orderToUpdate = orderService.getOrderBySgId(order.getSgId());
    	orderToUpdate.setDeadline(order.getDeadline());
    	List<OrderLineEntity> newLines = new ArrayList<>();
    	List<OrderLineEntity> oldLines = orderToUpdate.getLines();
    	for(OrderLineEntity line : order.getLines()) {
    		// Try to get line
    		OrderLineEntity existingLine = orderService.getOrderLineBySgId(line.getSgId());
    		if(existingLine == null) {
    			// Create new line
    			newLines.add(line);
    		} else {
    			existingLine.setQuantity(line.getQuantity());
    			existingLine.setDescription(line.getDescription());
    			newLines.add(existingLine);
    			oldLines.remove(existingLine);
    		}
    	}
    	
    	// Remove existing oldLines
    	for(OrderLineEntity line : oldLines) {
    		line.setDeleted(true);
    		newLines.add(line);
    	}
    	order.setLines(newLines);
    	return order;
    }
    
    public String decrypt(String securedXml) {
    	JAXBElement<AMQSecureMessage> securedRoot = 
				(JAXBElement<AMQSecureMessage>) unmarshaller.unmarshal(new StringSource(securedXml));
    	String encryptedData = securedRoot.getValue().getData().getValue();
		return cryptoService.decrypt(encryptedData, cryptoService.getSGKey());
	}
    
    @Override
    public void onMessage(Message message) {
        try {
        	String securedXml = ((TextMessage)message).getText();
        	String xml = decrypt(securedXml);
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
					break;
				case UPDATE:
					order = toOrderEntity(solicitud);
					order = getOrderToUpdate(order);
					orderService.updateOrder(order);
					break;
			}
		} catch (JMSException e) {
			logger.error("'text' field not found at message");
		}
    }
    
}
