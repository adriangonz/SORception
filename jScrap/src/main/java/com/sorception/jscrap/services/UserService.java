/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.services;

import com.sorception.jscrap.dao.UserDAO;
import com.sorception.jscrap.models.UserEntity;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.transaction.annotation.Transactional;
import java.util.List;
import org.springframework.stereotype.Service;
/**
 *
 * @author kaseyo
 */
@Service
public class UserService {
    @Autowired
    private UserDAO userDAO;
    
    @Transactional
    public List<UserEntity> getAllUsers() {
        return userDAO.getAllUsers();
    }
    
    @Transactional
    public UserEntity addUser(String name) {
        UserEntity user = new UserEntity(name);
        Long id = userDAO.addUser(user);
        user.setId(id);
        return user;
    }
}
