/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.webservices;

import com.sorception.jscrap.error.ServiceUnavailableException;
import com.sorception.jscrap.entities.SettingsEntity;
import com.sorception.jscrap.error.ResourceNotFoundException;
import com.sorception.jscrap.generated.Desguace;
import com.sorception.jscrap.generated.GetState;
import com.sorception.jscrap.generated.GetStateResponse;
import com.sorception.jscrap.generated.ObjectFactory;
import com.sorception.jscrap.generated.SignUp;
import com.sorception.jscrap.generated.SignUpResponse;
import com.sorception.jscrap.services.SettingsService;
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
    
    private Desguace desguace() {
        SettingsEntity settings = settingsService.getGlobalSettings();
        Desguace desguace = new Desguace();
        desguace.setName(objectFactory.createString(settings.getName()));
        return desguace;
    }
    
    public Object marshalWithSoapActionHeader(Object d, final String action) {
        return webServiceTemplate.marshalSendAndReceive(d, new WebServiceMessageCallback() {
            @Override
            public void doWithMessage(WebServiceMessage message) {
                ((SoapMessage)message).setSoapAction("http://tempuri.org/" + action);
            }
        });
    }
    
    public String signUp() {
        SignUp signUpRequest = new SignUp();
        signUpRequest.setD(objectFactory.createDesguace(desguace()));
        SignUpResponse response = (SignUpResponse)
                this.marshalWithSoapActionHeader(signUpRequest, "IGestionDesguace/signUp");
        if("-1".equals(response))
            throw new ServiceUnavailableException("Web Service returned -1");
        return response.getSignUpResult().toString();
    }
    
    public String getState(String temporalToken) {
        GetState getStateRequest = new GetState();
        getStateRequest.setId(Integer.parseInt(temporalToken));
        GetStateResponse response = (GetStateResponse) 
                this.marshalWithSoapActionHeader(getStateRequest, "IGestionDesguace/getState");
        String state = response.getGetStateResult().toString();
        if("-1".equals(state))
            throw new ServiceUnavailableException("Web Service returned -1");
        return state;
    }
}
