/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.entities;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Table;

import com.fasterxml.jackson.annotation.JsonIgnore;

/**
 *
 * @author kaseyo
 */
@Entity
@Table(name="ApplicationUser")
public class UserEntity extends AbstractEntity {
    @Column(name = "name", nullable = false)
    private String name;
    
    @Column(name = "isAdmin", nullable = false)
    private Boolean admin = false;

    @Column(name = "username", unique = true, nullable = false)
    private String username;
    
    @Column(name = "password", nullable = false)
    private String password;
    
    public UserEntity() {}

    public UserEntity(String username, String name, String password) {
        this.username = username;
        this.name = name;
        this.admin = false;
        this.password = password;
    }

    public String getName() {
        return name;
    }
    
    public String getUsername() {
        return username;
    }
    
    public Boolean getIsAdmin() {
        return admin;
    }
    
    @JsonIgnore
    public String getPassword() {
    	return password;
    }
}
