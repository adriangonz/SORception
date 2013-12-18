package com.sorception.jscrap.config;

import org.springframework.context.annotation.*;
import org.springframework.beans.factory.config.PropertyPlaceholderConfigurer;
import org.springframework.core.io.ClassPathResource;

@Configuration
@ComponentScan(basePackages = { "com.sorception.jscrap" })
@ImportResource("classpath:spring-security-context.xml")
public class RootConfig {
	
	@Bean
	public static PropertyPlaceholderConfigurer propertyPlaceholderConfigurer() {
		PropertyPlaceholderConfigurer ppc = new PropertyPlaceholderConfigurer();
                ppc.setLocations(new ClassPathResource[] {
                    new ClassPathResource("/persistence.properties"),
                    new ClassPathResource("/jscrap.properties"),
                    new ClassPathResource("/webservice.properties"),
                    new ClassPathResource("/activemq.properties")
                });
		return ppc;
	}
	
}