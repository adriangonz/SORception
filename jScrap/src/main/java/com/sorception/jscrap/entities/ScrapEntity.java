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
@Entity
@Table(name = "Scrap")
public class ScrapEntity extends AbstractEntity {
    @Column(name = "Name")
    private String _name;

    public ScrapEntity() {}

    public ScrapEntity(String _name) {
        this._name = _name;
    }

    public String getName() {
        return _name;
    }
}
