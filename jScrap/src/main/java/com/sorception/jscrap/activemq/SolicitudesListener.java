/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.activemq;

import com.sorception.jscrap.services.TokenService;

import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.MessageListener;

import org.slf4j.LoggerFactory;

/**
 *
 * @author kaseyo
 */
public class SolicitudesListener implements MessageListener {

    final static org.slf4j.Logger logger = LoggerFactory.getLogger(SolicitudesListener.class);
    
    @Override
    public void onMessage(Message message) {
        try {
			logger.info("Text received: " + message.getStringProperty("text"));
		} catch (JMSException e) {
			logger.error("'text' field not found at message");
		}
    }
    
}
