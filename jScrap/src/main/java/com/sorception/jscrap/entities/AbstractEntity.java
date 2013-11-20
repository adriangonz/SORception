/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.entities;

import java.io.Serializable;
import java.util.Date;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import javax.persistence.PrePersist;
import javax.persistence.PreUpdate;
import javax.persistence.Temporal;
import javax.persistence.TemporalType;

/**
 *
 * @author kaseyo
 */
@Entity
abstract class AbstractEntity implements Serializable {
    @Id
    @Column(name = "Id")
    @GeneratedValue
    protected Long _id;
    
    @Column(name = "Created")
    @Temporal(TemporalType.TIMESTAMP)
    protected Date _created;
    
    @Temporal(TemporalType.TIMESTAMP)
    @Column(name = "Updated")
    protected Date _updated;

    protected AbstractEntity() {}
    
    @PrePersist
    protected void onCreate() {
        _updated = _created = new Date();
    }

    @PreUpdate
    protected void onUpdate() {
        _updated = new Date();
    }
    
    public void setId(Long id) {
        _id = id;
    }
    
    public Long getId() {
        return _id;
    }
    
    public Date getCreated() {
        return _created;
    }

    public Date getUpdated() {
        return _updated;
    }
}
