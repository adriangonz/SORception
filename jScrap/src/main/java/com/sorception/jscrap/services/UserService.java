/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.services;

import com.sorception.jscrap.dao.UserDAO;
import com.sorception.jscrap.entities.UserEntity;
import com.sorception.jscrap.error.AuthenticationException;
import com.sorception.jscrap.error.ResourceNotFoundException;
import com.sorception.jscrap.security.AuthenticationTokenUtils;
import java.util.HashMap;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.transaction.annotation.Transactional;
import java.util.List;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.stereotype.Service;
/**
 *
 * @author kaseyo
 */
@Service
@Transactional
public class UserService {
    @Autowired
    private UserDAO userDAO;
    
    @Autowired
    private AuthenticationManager authenticationManager;
    
    @Autowired
    private UserDetailsService userDetailsService;
    
    final static Logger logger = LoggerFactory.getLogger(TokenService.class);
    
    public List<UserEntity> getAllUsers() {
        logger.info("Obtenemos todos los usuarios");
        return userDAO.getAllUsers();
    }
    
    public UserEntity addUser(String username, String name) {
        UserEntity user = new UserEntity(username, name);
        return userDAO.addUser(user);
    }

    public UserEntity getUser(Long userId) {
        UserEntity user = userDAO.getUser(userId);
        if(null == user)
            throw new ResourceNotFoundException("User does not exist");
        return user;
    }
    
    public UserEntity getUserByUsername(String username) {
        UserEntity user = userDAO.getUserByUsername(username);
        if(null == user)
            throw new ResourceNotFoundException("User does not exist");
        return user;
    }

    public void removeUser(Long userId) {
        if(!userDAO.deleteUser(userId))
            throw new ResourceNotFoundException("User does not exist");
    }
    
    public UserEntity authenticateUser(String username, String password) {
        try {
            UsernamePasswordAuthenticationToken authenticationToken =
                new UsernamePasswordAuthenticationToken(username, password);
            Authentication authentication = this.authenticationManager.authenticate(authenticationToken);
            SecurityContextHolder.getContext().setAuthentication(authentication);
            return this.getUserByUsername(username);
        } catch (BadCredentialsException es) {
            throw new AuthenticationException("Bad credentials for user " + username);
        }
    }
}
