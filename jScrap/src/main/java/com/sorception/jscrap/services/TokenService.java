/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.services;

import com.google.common.base.Throwables;
import com.sorception.jscrap.dao.TokenDAO;
import com.sorception.jscrap.entities.TokenEntity;
import com.sorception.jscrap.error.ResourceNotFoundException;
import com.sorception.jscrap.webservices.SGClient;
import java.util.List;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

/**
 *
 * @author kaseyo
 */
@Service
@Transactional
public class TokenService {
    @Autowired
    TokenDAO tokenDAO;
    
    @Autowired
    SGClient sgClient;
    
    final static Logger logger = LoggerFactory.getLogger(TokenService.class);
    
    private TokenEntity saveValid(String token) {
        TokenEntity tokenEntity = new TokenEntity(
                token, TokenEntity.TokenStatus.VALID);
        return tokenDAO.save(tokenEntity);
    }
    
    public TokenEntity requestToken() {
        logger.info("Save valid token");
        // Access to web service
        String temporalToken = sgClient.signUp();
        // Save temporal token
        TokenEntity tokenEntity = new TokenEntity(
                temporalToken, TokenEntity.TokenStatus.REQUESTED);
        return tokenDAO.save(tokenEntity);
    }
    
    public TokenEntity getValid() {
        TokenEntity tokenEntity = tokenDAO.getValid();
        if(null == tokenEntity) {
            // Check if we have requested one
            tokenEntity = tokenDAO.getRequestOrTemporal();
            if(null == tokenEntity) // If not, throw 404
                throw new ResourceNotFoundException("Not valid token or request found");
            // Check if new token is available
            // Method getState will throw NotFound if not valid
            String newToken = sgClient.getState(tokenEntity.getToken());
            tokenEntity = this.saveValid(newToken);
        }
        return tokenEntity;
    }
    
    public List<TokenEntity> list() {
        return tokenDAO.list();
    }
}
