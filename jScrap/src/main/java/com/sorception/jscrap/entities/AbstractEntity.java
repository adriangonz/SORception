/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.entities;

import java.io.Serializable;
import java.util.Date;
import javax.persistence.Column;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import javax.persistence.Inheritance;
import javax.persistence.InheritanceType;
import javax.persistence.MappedSuperclass;
import javax.persistence.Temporal;
import javax.persistence.TemporalType;

/**
 *
 * @author kaseyo
 */
@MappedSuperclass
@Inheritance(strategy = InheritanceType.TABLE_PER_CLASS)
public abstract class AbstractEntity implements Serializable {
    @Id
    @Column(name = "id")
    @GeneratedValue
    protected Long _id;
    
    @Temporal(TemporalType.TIMESTAMP)
    @Column(name = "creationDate", nullable = false)
    final private Date _created;
    
    protected AbstractEntity() {
        _created = new Date();
    }
    
    public Date getCreationDate() {
        return _created;
    }
    
    public void setId(Long id) {
        _id = id;
    }
    
    public Long getId() {
        return _id;
    }
}