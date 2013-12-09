/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.entities;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Table;

/**
 *
 * @author kaseyo
 */
@Entity
@Table(name="User")
public class UserEntity extends AbstractEntity {
    @Column(name = "name")
    private String _name;
    
    @Column(name = "isAdmin")
    private Boolean _admin = false;

    public UserEntity() {}

    public UserEntity(String name) {
        _name = name;
    }

    public String getName() {
        return _name;
    }
    
    public Boolean getIsAdmin() {
        return _admin;
    }
}
