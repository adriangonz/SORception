/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.api;

import org.springframework.stereotype.Controller;

import com.sorception.jscrap.models.UserEntity;
import com.sorception.jscrap.services.UserService;
import java.util.List;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;

/**
 *
 * @author kaseyo
 */
@Controller
@RequestMapping("/api/user")
public class UserController {
    @Autowired
    private UserService userService;
    
    @RequestMapping(value="", method=RequestMethod.GET)
    public List<UserEntity> getUser() {
        return userService.getAllUsers();
    }
}
