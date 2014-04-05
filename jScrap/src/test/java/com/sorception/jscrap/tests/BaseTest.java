package com.sorception.jscrap.tests;

import static org.hamcrest.Matchers.*;
import static org.junit.Assert.*;

import org.junit.Test;
import org.junit.runner.RunWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.ApplicationContext;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.test.context.ContextConfiguration;
import org.springframework.test.context.TestExecutionListeners;
import org.springframework.test.context.junit4.SpringJUnit4ClassRunner;
import org.springframework.test.context.support.DependencyInjectionTestExecutionListener;
import org.springframework.test.context.support.DirtiesContextTestExecutionListener;
import org.springframework.test.context.transaction.TransactionConfiguration;
import org.springframework.test.context.transaction.TransactionalTestExecutionListener;
import org.springframework.test.context.web.WebAppConfiguration;
import org.springframework.transaction.annotation.Transactional;

import com.github.springtestdbunit.DbUnitTestExecutionListener;
import com.github.springtestdbunit.annotation.DbUnitConfiguration;
import com.sorception.jscrap.config.PersistenceConfig;
import com.sorception.jscrap.config.RootConfig;
import com.sorception.jscrap.config.SecurityConfig;
import com.sorception.jscrap.services.CustomUserDetailsService;

@RunWith(SpringJUnit4ClassRunner.class)
@WebAppConfiguration
@ContextConfiguration(classes = {
		RootConfig.class,
		TestConfig.class,
		PersistenceConfig.class,
		SecurityConfig.class,
		CustomUserDetailsService.class
})
@TestExecutionListeners({ 
	DependencyInjectionTestExecutionListener.class,
    DirtiesContextTestExecutionListener.class,
    TransactionalTestExecutionListener.class,
    DbUnitTestExecutionListener.class })
@DbUnitConfiguration(databaseConnection = "customDbUnitDatabaseConnection")
@TransactionConfiguration(defaultRollback=true)
@Transactional
public class BaseTest {
	
	@Autowired
	protected ApplicationContext applicationContext;
	
	@Autowired
	UserDetailsService userDetailsService;
	
	@Test
	public void ApplicationContext_Autowired_ShouldNotBeNull() {
		assertThat(applicationContext, is(notNullValue()));
	}
		
	protected void loginUser(String username) {
		Authentication authToken = getPrincipal(username);
		SecurityContextHolder.getContext().setAuthentication(authToken);
	}
	
	protected Authentication getPrincipal(String username) {
		UserDetails userDetails = userDetailsService.loadUserByUsername (username);
		Authentication authToken = new UsernamePasswordAuthenticationToken (userDetails.getUsername(), userDetails.getPassword(), userDetails.getAuthorities());
        return authToken;
	}
}
