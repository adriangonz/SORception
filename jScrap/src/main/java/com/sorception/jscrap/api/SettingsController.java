/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.api;

import com.sorception.jscrap.entities.SettingsEntity;
import com.sorception.jscrap.services.SettingsService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

/**
 *
 * @author kaseyo
 */
@Controller
@RequestMapping("/api/settings")
public class SettingsController {
    @Autowired
    SettingsService settingsService;
    
    @RequestMapping("/")
    @ResponseBody
    public SettingsEntity getSettings() {
        return settingsService.getGlobalSettings();
    }
}
