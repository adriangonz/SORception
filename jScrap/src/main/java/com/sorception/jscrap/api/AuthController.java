/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.api;

import static com.sorception.jscrap.api.UserController.logger;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;

/**
 *
 * @author kaseyo
 */
@Controller
@RequestMapping("/api/auth")
public class AuthController {
    
    final static Logger logger = LoggerFactory.getLogger(AuthController.class);
    
    @RequestMapping(value="", method=RequestMethod.POST)
    @ResponseBody
    public String authenticateUser() {
        logger.info("hola" + " - " + "ke ase");
        return "hola";
        //return new UserTokenDTO(userService.authenticateUser(user.username, user.password));
    }
}
