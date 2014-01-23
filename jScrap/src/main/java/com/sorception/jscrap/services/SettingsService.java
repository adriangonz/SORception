/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.services;

import com.sorception.jscrap.entities.SettingsEntity;
import com.sorception.jscrap.entities.TokenEntity;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

/**
 *
 * @author kaseyo
 */
@Service
public class SettingsService {
	@Autowired
    private SettingsEntity globalSettings;
    
    @Autowired
    private TokenService tokenService;
    
    public SettingsEntity getGlobalSettings() {
        return globalSettings;
    }
    
    public SettingsEntity getExtendedSettings() {
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
