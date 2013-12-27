/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.security;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.crypto.codec.Base64;
import org.springframework.security.web.AuthenticationEntryPoint;
import org.springframework.security.web.authentication.www.BasicAuthenticationEntryPoint;
import org.springframework.security.web.authentication.www.BasicAuthenticationFilter;
import org.springframework.security.web.authentication.www.DigestAuthenticationFilter;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.GenericFilterBean;

/**
 *
 * @author kaseyo
 */
public class RestSecurityFilter extends GenericFilterBean {

    final static Logger logger = LoggerFactory.getLogger(RestSecurityFilter.class);
    
    private AuthenticationManager authenticationManager;
    private AuthenticationEntryPoint authenticationEntryPoint;
    
    public RestSecurityFilter(AuthenticationManager authenticationManager) {
        this(authenticationManager, new BasicAuthenticationEntryPoint());
    }

    public RestSecurityFilter(AuthenticationManager authenticationManager, AuthenticationEntryPoint authenticationEntryPoint) {
        this.authenticationManager = authenticationManager;
        this.authenticationEntryPoint = authenticationEntryPoint;
    }
    
    @Override
    public void doFilter(ServletRequest req, ServletResponse resp, FilterChain chain) throws IOException, ServletException {
        HttpServletRequest request = (HttpServletRequest) req;
        HttpServletResponse response = (HttpServletResponse) resp;

        //Pull out the Authorization header
        String authorization = request.getHeader("Authorization");

        //If the Authorization header is null, let the ExceptionTranslationFilter provided by
        //the <http> namespace kick of the BasicAuthenticationEntryPoint to provide the username and password dialog box
        if (authorization == null) {
            logger.info(request.getMethod());
            chain.doFilter(request, response);
            return;
        }

        String[] credentials = decodeHeader(authorization);
        assert credentials.length == 2;
        Authentication authentication = new RestToken(credentials[0], credentials[1]);

        try {
            //Request the authentication manager to authenticate the token
            Authentication successfulAuthentication = authenticationManager.authenticate(authentication);
            //Pass the successful token to the SecurityHolder where it can be retrieved by this thread at any stage.
            SecurityContextHolder.getContext().setAuthentication(successfulAuthentication);
            //Continue with the Filters
            chain.doFilter(request, response);
        } catch (AuthenticationException authenticationException) {
            //If it fails clear this threads context and kick off the authentication entry point process.
            SecurityContextHolder.clearContext();
            authenticationEntryPoint.commence(request, response, authenticationException);
        }
    }
    
    public String[] decodeHeader(String authorization) {
        //Decode the Auth Header to get the username and password
        try {
            byte[] decoded = Base64.decode(authorization.substring(6).getBytes("UTF-8"));
            String credentials = new String(decoded);
            return credentials.split(":");
        } catch (UnsupportedEncodingException e) {
            throw new UnsupportedOperationException(e);
        }
    }
}
