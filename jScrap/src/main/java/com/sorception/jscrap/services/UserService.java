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
import org.springframework.security.authentication.dao.SaltSource;
import org.springframework.security.authentication.encoding.ShaPasswordEncoder;
import org.springframework.security.core.userdetails.UserDetails;
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
    
    @Autowired
    private SaltSource saltSource;
    
    @Autowired
    private ShaPasswordEncoder passwordEncoder;
    
    public UserService() {
		super(UserEntity.class);
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
        encodePassword(user);
        create(user);
        return user;
    }
	
	//TODO: Only owner or admin
    public UserEntity getUser(Long userId) {
        return findOne(userId);
    }
    
    //TODO: Only owner or admin
    public UserEntity getUserByUsername(String username) {
        UserEntity user = ((IUserDAO)getDao()).findByUsername(username);
        if(null == user)
            throw new ResourceNotFoundException("User " + username + " was not found");
        return user;
    }
    
    //TODO: Only owner or admin
    public void removeUser(Long userId) {
    	delete(userId);
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
    
    private void encodePassword(UserEntity user) {
    	CustomUserDetailsService userDetailsService = new CustomUserDetailsService();
        UserDetails userDetails = userDetailsService.loadUserByCustomUser(user);
        String encodedPassword = 
        		passwordEncoder.encodePassword(
        				user.getPassword(), saltSource.getSalt(userDetails));
        user.setPassword(encodedPassword);
    }
    
    private String getAuthentication(String username, String password) {
		String creds = username + ":" + password;
		creds = Base64.encodeBase64String(creds.getBytes()).trim();
		return "Basic " + creds;
    }
}
