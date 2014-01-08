/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.services;

import com.sorception.jscrap.entities.UserEntity;
import com.sorception.jscrap.error.ResourceNotFoundException;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.GrantedAuthoritiesContainer;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

/**
 *
 * @author kaseyo
 */

@Service("userDetailsService")
public class CustomUserDetailsService implements UserDetailsService {
    
    @Autowired
    UserService userService;
    
    final static Logger logger = LoggerFactory.getLogger(CustomUserDetailsService.class);
    
    private List<GrantedAuthority> getAuthorities(UserEntity userEntity) {
        ArrayList<GrantedAuthority> authorities = new ArrayList<GrantedAuthority>();
        if(userEntity.getIsAdmin()) {
            authorities.add(new SimpleGrantedAuthority("ROLE_ADMIN"));
        }
        authorities.add(new SimpleGrantedAuthority("ROLE_USER"));
        return authorities;
    }
    
    @Override
    @Transactional(readOnly = true)
    public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
        try {
            UserEntity userEntity = this.userService.getUserByUsername(username);   
            return new User(userEntity.getUsername(), "", getAuthorities(userEntity));
        } catch(ResourceNotFoundException ex) {
            throw new UsernameNotFoundException(username);
        }
    }
    
}
