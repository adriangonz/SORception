/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.api;

import com.sorception.jscrap.entities.TokenEntity;
import com.sorception.jscrap.services.TokenService;
import java.util.List;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpMethod;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.ResponseStatus;

/**
 *
 * @author kaseyo
 */
@Controller
@RequestMapping("/api/token")
public class TokenController {
    @Autowired
    TokenService tokenService;
    
    @RequestMapping("/")
    @ResponseBody
    public TokenEntity getValid() {
        return tokenService.getValid();
    }
    
    @RequestMapping(value = "/", method = RequestMethod.POST)
    @ResponseStatus(HttpStatus.OK)
    public void requestToken() {
        tokenService.saveRequest();
    }
    
    @RequestMapping(value = "/", method = RequestMethod.PUT)
    @ResponseStatus(HttpStatus.OK)
    public void saveToken() {
        tokenService.saveValid("NewToken");
    }
    
    @RequestMapping("/list")
    @ResponseBody
    public List<TokenEntity> list() {
        return tokenService.list();
    }
}
