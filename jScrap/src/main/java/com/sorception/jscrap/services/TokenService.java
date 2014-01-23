/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.services;

import com.sorception.jscrap.dao.IGenericDAO;
import com.sorception.jscrap.dao.ITokenDAO;
import com.sorception.jscrap.entities.TokenEntity;
import com.sorception.jscrap.error.ResourceNotFoundException;
import com.sorception.jscrap.webservices.SGClient;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

/**
 *
 * @author kaseyo
 */
@Service
@Transactional(noRollbackFor={ResourceNotFoundException.class})
public class TokenService extends AbstractService<TokenEntity> {
	@Autowired
    private SGClient sgClient;
    
    @Autowired
    private ActiveMQService activeMQService;
    
    @Autowired
    private ITokenDAO dao;
    
    public TokenService() {
		super(TokenEntity.class);
	}
    
	@Override
	protected IGenericDAO<TokenEntity> getDao() {
		return dao;
	}
        
    public TokenEntity requestToken() {
        logger.info("New token requested. Disabling old ones...");
        getTokenDao().invalidateTokens();
        // Access to web service
        TokenEntity temporalToken = sgClient.signUp();
        // Disable jmsContainer
        activeMQService.disableJmsContainers();
        // Save temporal token
        return create(temporalToken);
    }
    
    public TokenEntity getValid() {
        TokenEntity tokenEntity = getTokenDao().findByStatus(TokenEntity.TokenStatus.VALID);
        if(null == tokenEntity) {
            // Check if we have requested one
            tokenEntity = getRequestOrTemporal();
            if(null == tokenEntity) // If not, throw 404
                throw new ResourceNotFoundException("Not valid token or request found");
            // Check if new token is available
            // Method getState will throw NotFound if not valid
            TokenEntity newToken = sgClient.getState(tokenEntity.getToken());
            tokenEntity = create(newToken);
            if(!tokenEntity.isValid()) // If not, throw 404
                throw new ResourceNotFoundException("Token request has not been accepted");
            else // Enable JmsContainer
            	activeMQService.enableJmsContainers(tokenEntity);
        }
        return tokenEntity;
    }
    
    public List<TokenEntity> list() {
        return findAll();
    }
    
    protected ITokenDAO getTokenDao() {
    	return ((ITokenDAO)getDao());
    }
    
    protected TokenEntity getRequestOrTemporal() {
    	List<TokenEntity> tokens = getTokenDao()
    			.findByStatus(TokenEntity.TokenStatus.REQUESTED, TokenEntity.TokenStatus.TEMPORAL);
    	if(tokens.size() > 0)
    		return tokens.get(0);
    	else
    		return null;
    }
}
