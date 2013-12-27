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
import com.sorception.jscrap.security.RestToken;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.transaction.annotation.Transactional;
import java.util.List;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.security.access.annotation.Secured;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
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
    
    final static Logger logger = LoggerFactory.getLogger(UserService.class);
    
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
        logger.info("Buscando al usuario " + username + "...");
        UserEntity user = userDAO.getUserByUsername(username);
        logger.info("Resultados...");
        if(null == user) {
            logger.info("El usuario " + username + " es null!");
            throw new ResourceNotFoundException("User does not exist");
        } else {
            logger.info(user.getName());
        }
        return user;
    }

    public void removeUser(Long userId) {
        if(!userDAO.deleteUser(userId))
            throw new ResourceNotFoundException("User does not exist");
    }
    
    public Authentication authenticateUser(String username, String password) {
        try {
            logger.info("Generating auth token for user "+ username + "...");
            UsernamePasswordAuthenticationToken token = new UsernamePasswordAuthenticationToken(username, password);
            Authentication authentication = this.authenticationManager.authenticate(token);
            return authentication;
        } catch (BadCredentialsException es) {
            throw new AuthenticationException("Bad credentials for user " + username);
        }
    }
}
