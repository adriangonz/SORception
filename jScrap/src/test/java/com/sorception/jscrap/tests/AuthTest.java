package com.sorception.jscrap.tests;

import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

import java.util.HashMap;
import java.util.Map;

import javax.annotation.Resource;

import org.apache.commons.net.util.Base64;
import org.junit.Before;
import org.junit.Test;
import org.springframework.http.MediaType;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.security.web.FilterChainProxy;
import org.springframework.security.web.csrf.CsrfToken;
import org.springframework.security.web.csrf.HttpSessionCsrfTokenRepository;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.ResultActions;
import org.springframework.test.web.servlet.request.MockHttpServletRequestBuilder;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;
import org.springframework.web.context.WebApplicationContext;

import com.github.springtestdbunit.annotation.DatabaseSetup;
import com.sorception.jscrap.services.UserService;

@DatabaseSetup("classpath:userDataset.xml")
public class AuthTest extends BaseTest {

	private MockMvc mockMvc;
	private UserService userService;
	
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
		makeRequestAs(null, "/api/user", getSampleUserAsJSON()).andExpect(status().isUnauthorized());
	}
	
	@Test
	public void createUser_notAdmin_ShouldReturnError() throws Exception {
		makeRequestAs("kaseyo", "/api/user", getSampleUserAsJSON()).andExpect(status().isForbidden());
	}
	
	@Test
	public void createUser_admin_ShouldWork() throws Exception {
		makeRequestAs("admin", "/api/user", getSampleUserAsJSON()).andExpect(status().isCreated());
	}
	
	@Test
	public void authenticate_notAuth_ShouldWork() throws Exception {
		makeRequestAs(null, "/api/user/authenticate", getCredentialsFor("kaseyo")).andExpect(status().isOk());
	}
	
	private ResultActions makeRequestAs(String username, String url, String json) throws Exception {
		CsrfToken csrfToken = getCsrfToken();
		Map map = new HashMap();
        map.put("org.springframework.security.web.csrf.HttpSessionCsrfTokenRepository.CSRF_TOKEN",
        		csrfToken);
		MockHttpServletRequestBuilder postRequest = post(url)
			.header("X-CSRF-TOKEN", csrfToken.getToken())
			.contentType(MediaType.APPLICATION_JSON)
			.content(json)
			.sessionAttrs(map);
		if(username != null)
			postRequest.header("Authorization", getAuthorizationHeader(username));
		ResultActions result = mockMvc.perform(postRequest);
		return result;
	}
	
	private String getSampleUserAsJSON() {
		return "{"
				+ "\"name\": \"Harry Potter\","
				+ "\"username\": \"harry\","
				+ "\"password\": \"mag1c\""
			+ "}"; 
	}
	
	private String getCredentialsFor(String username) {
		return "{"
				+ "\"username\": \"" + username + "\","
				+ "\"password\": \"password\""
			+ "}"; 
	}
	
	private String getAuthorizationHeader(String username) {
		return "Basic " + new String(Base64.encodeBase64((username + ":password").getBytes()));
	}
	
	private CsrfToken getCsrfToken() {
		HttpSessionCsrfTokenRepository httpSessionCsrfTokenRepository = new HttpSessionCsrfTokenRepository();
		CsrfToken csrfToken = httpSessionCsrfTokenRepository.generateToken(new MockHttpServletRequest());
		return csrfToken;
	}
}
