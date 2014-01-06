/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.entities;

import java.beans.Transient;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.EnumType;
import javax.persistence.Enumerated;
import javax.persistence.Table;

import org.junit.Ignore;

/**
 *
 * @author kaseyo
 */
@Entity
@Table(name = "Token")
public class TokenEntity extends AbstractEntity {
    @Column(name = "token")
    private String _token;
    
    @Column(name = "status")
    @Enumerated(EnumType.STRING)
    private TokenStatus _status;
    
    public enum TokenStatus {
        VALID,
        REQUESTED,
        EXPIRED,
        TEMPORAL;
    }
    
    public TokenEntity() {}
    
    public TokenEntity(String token, TokenStatus status){
        this._token = token;
        this._status = status;
    }
    
    public String getToken() {
        return this._token;
    }
    
    public TokenStatus getStatus() {
        return _status;
    }
    
    public Boolean isValid() {
    	return this._status == TokenStatus.VALID;
    }
}
