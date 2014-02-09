package com.sorception.jscrap.tests;

import javax.sql.DataSource;

import org.dbunit.ext.mysql.MySqlDataTypeFactory;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.jms.core.JmsTemplate;

import com.github.springtestdbunit.bean.DatabaseConfigBean;
import com.github.springtestdbunit.bean.DatabaseDataSourceConnectionFactoryBean;

@Configuration
public class TestConfig {
	@Autowired
	DataSource dataSource;
	
	@Mock
	JmsTemplate jmsTemplate;
	
	TestConfig() {
		MockitoAnnotations.initMocks(this);
	}
		
	@Bean
	public DatabaseConfigBean customDbUnitDatabaseConfig() {
		DatabaseConfigBean config = new DatabaseConfigBean();
		config.setDatatypeFactory(new MySqlDataTypeFactory());
		return config;
	}
		
	@Bean
	public DatabaseDataSourceConnectionFactoryBean customDbUnitDatabaseConnection() {
		DatabaseDataSourceConnectionFactoryBean connection =
				new DatabaseDataSourceConnectionFactoryBean();
		connection.setDatabaseConfig(customDbUnitDatabaseConfig());
		connection.setDataSource(dataSource);
		return connection;
	}
	
	@Bean
	public JmsTemplate jmsTemplate() {
		return jmsTemplate;
	}
}
