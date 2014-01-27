/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.config;

import com.sorception.jscrap.entities.SettingsEntity;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.PropertySource;

/**
 *
 * @author kaseyo
 */
@Configuration
@PropertySource("classpath:jscrap.properties")
public class ScrapConfig {
    @Value("${jscrap.name}")
    private String _name;
    
    @Bean
    public SettingsEntity globalSettings() {
        return new SettingsEntity(_name);
    }
}
