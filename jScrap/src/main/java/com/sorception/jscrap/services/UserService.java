/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.services;

import java.util.List;

import org.apache.commons.net.util.Base64;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.dao.IGenericDAO;
import com.sorception.jscrap.dao.IUserDAO;
import com.sorception.jscrap.dto.UserInfoDTO;
import com.sorception.jscrap.entities.UserEntity;
import com.sorception.jscrap.error.AuthenticationException;
import com.sorception.jscrap.error.ResourceNotFoundException;
/**
 *
 * @author kaseyo
 */

@Service
@Transactional
public class UserService extends AbstractService<UserEntity> {
	
    @Autowired
    private AuthenticationManager authenticationManager;
    
    @Autowired
    private IUserDAO dao;
    
    private PasswordEncoder encoder;
	
    public UserService() {
		super(UserEntity.class);
		encoder = new BCryptPasswordEncoder(256);
	}
    
	@Override
	protected IGenericDAO<UserEntity> getDao() {
		return dao;
	}
        
    public List<UserEntity> getAllUsers() {
        return findAll();
    }
    
    public UserEntity addUser(UserInfoDTO userInfo) {    	
        UserEntity user = new UserEntity(userInfo.username, userInfo.name, userInfo.password);
        create(user);
        return user;
    }

    public UserEntity getUser(Long userId) {
        return findOne(userId);
    }
    
    public UserEntity getUserByUsername(String username) {
        UserEntity user = ((IUserDAO)getDao()).findByUsername(username);
        if(null == user)
            throw new ResourceNotFoundException("User " + username + " was not found");
        return user;
    }

    public void removeUser(Long userId) {
    	delete(userId);
    }
    
    private String getAuthentication(String username, String password) {
		String creds = username + ":" + password;
		creds = Base64.encodeBase64String(creds.getBytes()).trim();
		return "Basic " + creds;
    }
    
    public String authenticateUser(String username, String password) {
        try {
            logger.info("Generating auth token for user "+ username + "...");
            UsernamePasswordAuthenticationToken token = new UsernamePasswordAuthenticationToken(username, password);
            this.authenticationManager.authenticate(token);
            return this.getAuthentication(username, password);
        } catch (BadCredentialsException es) {
            throw new AuthenticationException("Bad credentials for user " + username);
        }
    }
}
