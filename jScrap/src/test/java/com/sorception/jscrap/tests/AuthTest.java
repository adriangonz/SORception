package com.sorception.jscrap.tests;

import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

import javax.annotation.Resource;

import org.apache.commons.net.util.Base64;
import org.junit.Before;
import org.junit.Test;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.MediaType;
import org.springframework.mock.web.MockHttpSession;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContext;
import org.springframework.security.web.FilterChainProxy;
import org.springframework.security.web.context.HttpSessionSecurityContextRepository;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import org.springframework.web.context.WebApplicationContext;

import com.github.springtestdbunit.annotation.DatabaseSetup;
import com.sorception.jscrap.services.UserService;

@DatabaseSetup("classpath:userDataset.xml")
public class AuthTest extends BaseTest {

	private MockMvc mockMvc;
	private UserService userService;
	final Logger logger = LoggerFactory.getLogger(getClass());
	@Resource
    private FilterChainProxy springSecurityFilterChain;
	
	@Before
	public void setup() {
		mockMvc = MockMvcBuilders
				.webAppContextSetup((WebApplicationContext)applicationContext)
				.addFilter(springSecurityFilterChain)
				.build(); 
		userService = (UserService)applicationContext.getBean("userService");
	}
	
	@Test
	public void createUser_notAuth_ShouldReturnError() throws Exception {
		mockMvc
			.perform(
				post("/api/user")
					.contentType(MediaType.APPLICATION_JSON)
					.content(getSampleUserAsJSON()))
			.andExpect(status().isUnauthorized());
	}
	
	@Test
	public void createUser_notAdmin_ShouldReturnError() throws Exception {
		mockMvc
			.perform(
				post("/api/user")
					.contentType(MediaType.APPLICATION_JSON)
					.content(getSampleUserAsJSON())
					.header("Authorization", getAuthorizationHeader("kaseyo")))
			.andExpect(status().isForbidden());
	}
	
	@Test
	public void createUser_admin_ShouldWork() throws Exception {
		mockMvc
			.perform(
				post("/api/user")
					.contentType(MediaType.APPLICATION_JSON)
					.content(getSampleUserAsJSON())
					.header("Authorization", getAuthorizationHeader("admin")))
			.andExpect(status().isCreated());
	}
	
	private String getSampleUserAsJSON() {
		return "{"
				+ "\"name\": \"Harry Potter\","
				+ "\"username\": \"harry\","
				+ "\"password\": \"mag1c\""
			+ "}"; 
	}
	
	private String getAuthorizationHeader(String username) {
		return "Basic " + new String(Base64.encodeBase64((username + ":password").getBytes()));
	}
	
	public static class MockSecurityContext implements SecurityContext {

        private static final long serialVersionUID = -1386535243513362694L;

        private Authentication authentication;

        public MockSecurityContext(Authentication authentication) {
            this.authentication = authentication;
        }

        @Override
        public Authentication getAuthentication() {
            return this.authentication;
        }

        @Override
        public void setAuthentication(Authentication authentication) {
            this.authentication = authentication;
        }
    }
	
	private MockHttpSession getAuthenticatedSession(String username) {
		Authentication principal = this.getPrincipal(username);
        MockHttpSession session = new MockHttpSession();
        session.setAttribute(
                HttpSessionSecurityContextRepository.SPRING_SECURITY_CONTEXT_KEY, 
                new MockSecurityContext(principal));
        return session;
	}
}
