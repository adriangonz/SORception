/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.models;

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
@Table(name="User")
public class UserEntity {
    @Id
    @Column(name="Id")
    @GeneratedValue
    private Long _id;
    
    @Column(name="Name")
    private String _name;

    protected UserEntity() {}
    
    public UserEntity(String name) {
        _name = name;
    }
    
    public void setId(Long id) {
        _id = id;
    }
    
    public Long getId() {
        return _id;
    }
    
    public String getName() {
        return _name;
    }
}
