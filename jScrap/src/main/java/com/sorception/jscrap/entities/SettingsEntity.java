/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.entities;

import java.io.Serializable;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import javax.persistence.Table;


/**
 *
 * @author kaseyo
 */
public class SettingsEntity {
    // Dummy entity which holds information.
    // It is not persisted in the database,
    // because it is loaded from jscrap.properties
    final private String _name;

    public SettingsEntity(String _name) {
        this._name = _name;
    }

    public String getName() {
        return _name;
    }
}
