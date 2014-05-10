/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.config;

import org.apache.commons.configuration.ConfigurationException;
import org.apache.juddi.v3.client.config.UDDIClient;
import org.apache.juddi.v3.client.transport.Transport;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.oxm.jaxb.Jaxb2Marshaller;
import org.springframework.ws.client.core.WebServiceTemplate;
import org.springframework.ws.soap.SoapVersion;
import org.springframework.ws.soap.saaj.SaajSoapMessageFactory;
import org.uddi.api_v3.BindingDetail;
import org.uddi.api_v3.BindingTemplate;
import org.uddi.api_v3.BusinessService;
import org.uddi.api_v3.FindService;
import org.uddi.api_v3.GetBindingDetail;
import org.uddi.api_v3.GetServiceDetail;
import org.uddi.api_v3.Name;
import org.uddi.api_v3.ServiceDetail;
import org.uddi.api_v3.ServiceInfo;
import org.uddi.api_v3.ServiceList;
import org.uddi.v3_service.UDDIInquiryPortType;

import com.sorception.jscrap.generated.ObjectFactory;

/**
 *
 * @author kaseyo
 */
@Configuration
public class WebServiceConfig {

	@Value("${webservice.serviceName}")
    private String serviceName;
	
	@Value("${webservice.inquiryUrl}")
    private String inquiryUrl;
	
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
    
    private String findService(String name) throws Exception {
    	FindService fs = new FindService();
    	fs.getName().add(new Name(serviceName, "es"));
        ServiceList serviceList = inquiryService().findService(fs);
        
        return getAccessPoint(serviceList.getServiceInfos().getServiceInfo().get(0));
    }
    
    private String getAccessPoint(ServiceInfo serviceInfo) throws Exception {
    	UDDIInquiryPortType inquiryService = inquiryService();
    	GetServiceDetail gsd=new GetServiceDetail();
    	
    	gsd.getServiceKey().add(serviceInfo.getServiceKey());
        String servicekey = serviceInfo.getServiceKey();

        GetServiceDetail getServiceDetail=new GetServiceDetail();
        //getServiceDetail.setAuthInfo(authinfo);
        getServiceDetail.getServiceKey().add(servicekey);
        ServiceDetail serviceDetail=inquiryService.getServiceDetail(getServiceDetail);
        BusinessService businessservice=serviceDetail.getBusinessService().get(0);
        String bindingkey = businessservice.getBindingTemplates().getBindingTemplate().get(0).getBindingKey();

        GetBindingDetail gbd = new GetBindingDetail();
        //gbd.setAuthInfo(authinfo);
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
    		url = defaultUrl; 
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
