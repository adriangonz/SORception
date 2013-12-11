/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.services;

import com.sorception.jscrap.dao.UserDAO;
import com.sorception.jscrap.entities.UserEntity;
import com.sorception.jscrap.error.ResourceNotFoundException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.transaction.annotation.Transactional;
import java.util.List;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
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

    public void removeUser(Long userId) {
        if(!userDAO.deleteUser(userId))
            throw new ResourceNotFoundException("User does not exist");
    }
}
