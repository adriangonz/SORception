/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.webservices;

import com.sorception.jscrap.error.ServiceUnavailableException;
import com.sorception.jscrap.entities.SettingsEntity;
import com.sorception.jscrap.error.ResourceNotFoundException;
import com.sorception.jscrap.generated.ExposedDesguace;
import com.sorception.jscrap.generated.GetState;
import com.sorception.jscrap.generated.GetStateResponse;
import com.sorception.jscrap.generated.ObjectFactory;
import com.sorception.jscrap.generated.SignUp;
import com.sorception.jscrap.generated.SignUpResponse;
import com.sorception.jscrap.services.SettingsService;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.net.URI;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.ws.WebServiceMessage;
import org.springframework.ws.client.core.WebServiceMessageCallback;
import org.springframework.ws.client.core.WebServiceTemplate;
import org.springframework.ws.client.core.support.WebServiceGatewaySupport;
import org.springframework.ws.soap.SoapMessage;

/**
 *
 * @author kaseyo
 */
@Service
public class SGClient extends WebServiceGatewaySupport {
    
    @Autowired
    SettingsService settingsService;
    
    @Autowired
    ObjectFactory objectFactory;
    
    @Autowired
    WebServiceTemplate webServiceTemplate;
    
    private ExposedDesguace desguace() {
        SettingsEntity settings = settingsService.getGlobalSettings();
        ExposedDesguace desguace = objectFactory.createExposedDesguace();
        desguace.setName(objectFactory.createExposedDesguaceName(settings.getName()));
        return desguace;
    }
    
    public Object marshalWithSoapActionHeader(Object d, final String action) {
        return webServiceTemplate.marshalSendAndReceive(d, new WebServiceMessageCallback() {
            @Override
            public void doWithMessage(WebServiceMessage message) {
                String url = "";
                try {
                    URI uri = new URI(webServiceTemplate.getDefaultUri());
                    url = uri.getScheme() + "://" + uri.getHost() + "/";
                } catch (Exception ex) {
                    url = "http://tempuri.org/";
                } finally {
                    url += action;
                }
                logger.info("Accediendo a WS con Action: " + url + "...");
                ((SoapMessage)message).setSoapAction(url);
            }
        });
    }
    
    public String signUp() {
        SignUp signUpRequest = objectFactory.createSignUp();
        signUpRequest.setD(objectFactory.createSignUpD(desguace()));
        SignUpResponse response = (SignUpResponse)
                this.marshalWithSoapActionHeader(signUpRequest, "IGestionDesguace/signUp");
        String temporalToken = response.getSignUpResult().toString();
        if("-1".equals(temporalToken))
            throw new ServiceUnavailableException("Web Service returned -1");
        return temporalToken;
    }
    
    public String getState(String temporalToken) {
        GetState getStateRequest = objectFactory.createGetState();
        getStateRequest.setId(Integer.parseInt(temporalToken));
        GetStateResponse response = (GetStateResponse) 
                this.marshalWithSoapActionHeader(getStateRequest, "IGestionDesguace/getState");
        String state = response.getGetStateResult().toString();
        if("-1".equals(state))
            throw new ServiceUnavailableException("Web Service returned -1");
        return state;
    }
}