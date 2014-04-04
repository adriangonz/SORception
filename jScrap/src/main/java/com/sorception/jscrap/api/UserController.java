/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.api;

import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.ResponseStatus;

import com.sorception.jscrap.dto.UserCredentialsDTO;
import com.sorception.jscrap.dto.UserInfoDTO;
import com.sorception.jscrap.entities.UserEntity;
import com.sorception.jscrap.services.UserService;

/**
 *
 * @author kaseyo
 */

@Controller
@RequestMapping("/api/user")
public class UserController {
    
    final static Logger logger = LoggerFactory.getLogger(UserController.class);
    
    @Autowired
    private UserService userService;
    
    @RequestMapping(value="/authenticate", method=RequestMethod.POST)
    @ResponseBody
    public String authenticateUser(
            @RequestBody UserCredentialsDTO userCredentials) {
        return userService.authenticateUser(
                userCredentials.username, userCredentials.password);
    }
    
    @RequestMapping(value="", method=RequestMethod.GET)
    @ResponseBody
    public List<UserEntity> listUsers() {
        return userService.getAllUsers();
    }
    
    @RequestMapping(value="/{userId}", method=RequestMethod.GET)
    @ResponseBody
    public UserEntity getUser(@PathVariable Long userId) {
        return userService.getUser(userId);
    }
    
    @PreAuthorize("hasRole('ROLE_ADMIN')")
    @RequestMapping(value="", method=RequestMethod.POST)
    @ResponseBody
    @ResponseStatus(HttpStatus.CREATED)
    public UserEntity addUser(@RequestBody UserInfoDTO user) {
        return userService.addUser(user);
    }
    
    @PreAuthorize("hasRole('ROLE_ADMIN')")
    @RequestMapping(value="/{userId}", method=RequestMethod.DELETE)
    @ResponseBody
    @ResponseStatus(HttpStatus.OK)
    public void deleteUser(@PathVariable Long userId) {
        userService.removeUser(userId);
    }
}
