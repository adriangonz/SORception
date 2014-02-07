/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.api;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import com.sorception.jscrap.entities.SettingsEntity;
import com.sorception.jscrap.services.SettingsService;

/**
 *
 * @author kaseyo
 */
@Controller
@RequestMapping("/api/settings")
public class SettingsController {
    @Autowired
    SettingsService settingsService;
    
    @RequestMapping("")
    @ResponseBody
    public SettingsEntity getSettings() {
    	try {
			Thread.sleep(60000);
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
        return settingsService.getExtendedSettings();
    }
}
