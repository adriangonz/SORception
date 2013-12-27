/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.security;

import com.sorception.jscrap.services.CustomUserDetailsService;
import java.util.ArrayList;
import java.util.List;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.AuthenticationProvider;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;

/**
 *
 * @author kaseyo
 */
public class RestAuthenticationProvider implements AuthenticationProvider {

    final static Logger logger = LoggerFactory.getLogger(RestAuthenticationProvider.class);
    
    @Autowired
    CustomUserDetailsService userDetailsService;
    
    @Override
    public Authentication authenticate(Authentication authentication) throws AuthenticationException {
        RestToken restToken = (RestToken) authentication;

        String key = restToken.getKey();
        String credentials = restToken.getCredentials();

        return getAuthenticatedUser(key, credentials);
    }
    
    private Authentication getAuthenticatedUser(String key, String credentials) throws AuthenticationException {        
        UserDetails user = userDetailsService.loadUserByUsername(key);
        
        if(!user.getPassword().equals(credentials))
            throw new BadCredentialsException("Invalid credentials for user " + user);
        
        return new RestToken(key, credentials, user.getAuthorities());
    }

    @Override
    public boolean supports(Class<?> authentication) {
        return RestToken.class.equals(authentication);
    }
    
}
