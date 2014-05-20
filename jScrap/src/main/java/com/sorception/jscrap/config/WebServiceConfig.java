/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.config;

import org.apache.commons.configuration.ConfigurationException;
import org.apache.juddi.v3.client.UDDIConstants;
import org.apache.juddi.v3.client.config.UDDIClient;
import org.apache.juddi.v3.client.transport.Transport;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.oxm.jaxb.Jaxb2Marshaller;
import org.springframework.ws.client.core.WebServiceTemplate;
import org.springframework.ws.soap.SoapVersion;
import org.springframework.ws.soap.saaj.SaajSoapMessageFactory;
import org.uddi.api_v3.AuthToken;
import org.uddi.api_v3.BindingDetail;
import org.uddi.api_v3.BindingTemplate;
import org.uddi.api_v3.BusinessService;
import org.uddi.api_v3.FindQualifiers;
import org.uddi.api_v3.FindService;
import org.uddi.api_v3.GetAuthToken;
import org.uddi.api_v3.GetBindingDetail;
import org.uddi.api_v3.GetServiceDetail;
import org.uddi.api_v3.Name;
import org.uddi.api_v3.ServiceDetail;
import org.uddi.api_v3.ServiceInfo;
import org.uddi.api_v3.ServiceList;
import org.uddi.v3_service.UDDIInquiryPortType;
import org.uddi.v3_service.UDDISecurityPortType;

import com.sorception.jscrap.generated.ObjectFactory;

/**
 *
 * @author kaseyo
 */
@Configuration
public class WebServiceConfig {

	@Value("${webservice.serviceName}")
    private String serviceName;
	
	@Value("${webservice.juddiUser}")
    private String juddiUser;
	
	@Value("${webservice.juddiPassword}")
    private String juddiPassword;
	
    @Value("${webservice.defaultUrl}")
    private String defaultUrl;
    
    @Bean
    public Jaxb2Marshaller marshaller() {
        Jaxb2Marshaller marshaller = new Jaxb2Marshaller();
        marshaller.setContextPath("com.sorception.jscrap.generated");
        return marshaller;
    } 
    
    @Bean
    public Jaxb2Marshaller unmarshaller() {
        Jaxb2Marshaller marshaller = new Jaxb2Marshaller();
        marshaller.setContextPath("com.sorception.jscrap.generated");
        return marshaller;
    }
    
    @Bean
    public ObjectFactory objectFactory() {
        return new ObjectFactory();
    }
    
    @Bean
    public SaajSoapMessageFactory messageFactory() {
        SaajSoapMessageFactory messageFactory = new SaajSoapMessageFactory();
        messageFactory.setSoapVersion(SoapVersion.SOAP_11);
        return messageFactory;
    }
    
    @Bean
    public UDDIClient uddiClient() throws ConfigurationException {
    	UDDIClient uddiClient = new UDDIClient("uddi.xml");
    	return uddiClient;
    }
    
    public UDDIInquiryPortType inquiryService() throws Exception {
    	UDDIClient uddiClient = uddiClient();
    	Transport transport = uddiClient.getTransport("default");
    	return transport.getUDDIInquiryService();
    }
    
    public UDDISecurityPortType securityService() throws Exception {
    	UDDIClient uddiClient = uddiClient();
    	Transport transport = uddiClient.getTransport("default");
    	return transport.getUDDISecurityService();
    }
    
    private AuthToken authToken() throws Exception {
    	GetAuthToken getter = new GetAuthToken();
    	getter.setUserID(juddiUser);
    	getter.setCred(juddiPassword);
    	return securityService().getAuthToken(getter);
    }
    
    private String findService(String query) throws Exception {
    	AuthToken authToken = authToken();
    	FindService fs = new FindService();
    	
    	Name name = new Name();
    	name.setValue(query);
    	fs.getName().add(name);

    	fs.setAuthInfo(authToken.getAuthInfo());
    	
    	FindQualifiers qualifiers = new FindQualifiers();
    	qualifiers.getFindQualifier().add(UDDIConstants.EXACT_MATCH);
    	fs.setFindQualifiers(qualifiers);
    	
        ServiceList serviceList = inquiryService().findService(fs);
        ServiceInfo firstServiceInfo = serviceList.getServiceInfos().getServiceInfo().get(0); 
        return getAccessPoint(
        		firstServiceInfo,
        		authToken
        		);
    }
    
    private String getAccessPoint(ServiceInfo serviceInfo, AuthToken authToken) throws Exception {
    	UDDIInquiryPortType inquiryService = inquiryService();
    	GetServiceDetail gsd=new GetServiceDetail();
    	
    	gsd.getServiceKey().add(serviceInfo.getServiceKey());
        String servicekey = serviceInfo.getServiceKey();

        GetServiceDetail getServiceDetail=new GetServiceDetail();
        getServiceDetail.setAuthInfo(authToken.getAuthInfo());
        getServiceDetail.getServiceKey().add(servicekey);
        ServiceDetail serviceDetail=inquiryService.getServiceDetail(getServiceDetail);
        BusinessService businessservice=serviceDetail.getBusinessService().get(0);
        String bindingkey = businessservice.getBindingTemplates().getBindingTemplate().get(0).getBindingKey();

        GetBindingDetail gbd = new GetBindingDetail();
        gbd.setAuthInfo(authToken.getAuthInfo());
        gbd.getBindingKey().add(bindingkey);
        BindingDetail bindingdetail=inquiryService.getBindingDetail(gbd);
        BindingTemplate bindingtemplate=bindingdetail.getBindingTemplate().get(0);
        String accesspoint=bindingtemplate.getAccessPoint().getValue();
        
        return accesspoint;
    }
    
    @Bean
    public String webServiceUrl() {
    	String url = "";
    	try {
    		url = findService(serviceName);
    	} catch(Exception ex) {
    		url = ex.getMessage();
    	}
    	return url;
    }

    @Bean
    public WebServiceTemplate webServiceTemplate() {
        WebServiceTemplate webServiceTemplate = new WebServiceTemplate();
        webServiceTemplate.setMarshaller(marshaller());
        webServiceTemplate.setUnmarshaller(unmarshaller());
        webServiceTemplate.setDefaultUri(webServiceUrl());
        webServiceTemplate.setMessageFactory(messageFactory());
        return webServiceTemplate;
    }
}
