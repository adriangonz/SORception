/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.stereotype.Service;

import com.sorception.jscrap.entities.SettingsEntity;
import com.sorception.jscrap.entities.TokenEntity;

/**
 *
 * @author kaseyo
 */
@Service
@PreAuthorize("hasRole('ROLE_ADMIN')")
public class SettingsService {
	@Autowired
    private SettingsEntity globalSettings;
    
    @Autowired
    private TokenService tokenService;
    
    @PreAuthorize("hasRole('ROLE_USER')")
    public SettingsEntity getGlobalSettings() {
        return globalSettings;
    }
    
    @PreAuthorize("hasRole('ROLE_USER')")
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
