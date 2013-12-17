/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.api;

import org.springframework.stereotype.Controller;

import com.sorception.jscrap.entities.UserEntity;
import com.sorception.jscrap.services.UserService;
import java.util.List;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.ResponseStatus;

/**
 *
 * @author kaseyo
 */
class UserParamsDTO {
    public String name;
    public String username;
}

@Controller
@RequestMapping("/api/user")
public class UserController {
    @Autowired
    private UserService userService;
    
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
    
    @RequestMapping(value="", method=RequestMethod.POST)
    @ResponseBody
    @ResponseStatus(HttpStatus.CREATED)
    public UserEntity addUser(@RequestBody UserParamsDTO user) {
        return userService.addUser(user.username, user.name);
    }
    
    @RequestMapping(value="/{userId}", method=RequestMethod.DELETE)
    @ResponseBody
    @ResponseStatus(HttpStatus.OK)
    public void deleteUser(@PathVariable Long userId) {
        userService.removeUser(userId);
    }
}
