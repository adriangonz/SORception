/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.services;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.sorception.jscrap.entities.SettingsEntity;
import com.sorception.jscrap.entities.TokenEntity;

/**
 *
 * @author kaseyo
 */
@Service
public class SettingsService {

	final Logger logger = LoggerFactory.getLogger(getClass());
	
	@Autowired
    private SettingsEntity globalSettings;
    
    @Autowired
    private TokenService tokenService;
    
    public SettingsEntity getGlobalSettings() {
    	logger.info("Retrieving application settings." );
        return globalSettings;
    }
    
    public SettingsEntity getExtendedSettings() {
    	logger.info("Retrieving application settings and token info." );
        SettingsEntity settingsEntity = this.getGlobalSettings();
        try {
        	TokenEntity tokenEntity = tokenService.getValid();
            settingsEntity.setValidToken(tokenEntity);
        } catch (Exception ex) {
            settingsEntity.setValidToken(null);
        }
        settingsEntity.setTokenList(tokenService.list());
        return settingsEntity;
    }
}
