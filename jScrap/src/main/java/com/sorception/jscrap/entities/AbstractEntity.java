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
import javax.persistence.PrePersist;
import javax.persistence.PreUpdate;
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
    private Date _created;
    
    @Temporal(TemporalType.TIMESTAMP)
    @Column(name = "updateDate", nullable = false)
    private Date _updated;
    
    protected AbstractEntity() {}
    
    @PrePersist
    protected void onCreate() {
        _updated = _created = new Date();
    }

    @PreUpdate
    protected void onUpdate() {
        _updated = new Date();
    }
    
    public Date getCreationDate() {
        return _created;
    }
    
    public Date getUpdatedDate() {
        return _updated;
    }
    
    public Long getId() {
        return _id;
    }
}