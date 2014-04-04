/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.api;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;

import com.sorception.jscrap.entities.TokenEntity;
import com.sorception.jscrap.services.TokenService;

/**
 *
 * @author kaseyo
 */
@Controller
@RequestMapping("/api/token")
public class TokenController {
    @Autowired
    TokenService tokenService;
    
    @RequestMapping(value = "", method = RequestMethod.GET)
    @ResponseBody
    public TokenEntity getValid() {
        return tokenService.getValid();
    }
    
    @PreAuthorize("hasRole('ROLE_ADMIN')")
    @RequestMapping(value = "", method = RequestMethod.POST)
    @ResponseBody
    public TokenEntity requestToken() {
        return tokenService.requestToken();
    }
    
    @RequestMapping("/list")
    @ResponseBody
    public List<TokenEntity> list() {
        return tokenService.list();
    }
}
