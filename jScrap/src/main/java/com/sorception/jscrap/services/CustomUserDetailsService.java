/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.services;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.entities.CustomUserDetails;
import com.sorception.jscrap.entities.UserEntity;
import com.sorception.jscrap.error.ResourceNotFoundException;

/**
 *
 * @author kaseyo
 */

@Service("userDetailsService")
public class CustomUserDetailsService implements UserDetailsService {
    
    @Autowired
    UserService userService;
    
    final static Logger logger = LoggerFactory.getLogger(CustomUserDetailsService.class);
    
    @Override
    @Transactional(readOnly = true)
    public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
        try {
            UserEntity userEntity = this.userService.getUserByUsername(username);   
            return new CustomUserDetails(userEntity);
        } catch(ResourceNotFoundException ex) {
            throw new UsernameNotFoundException(username);
        }
    }
}
