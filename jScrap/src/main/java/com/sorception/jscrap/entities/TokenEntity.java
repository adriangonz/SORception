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

import com.fasterxml.jackson.annotation.JsonIgnore;

/**
 *
 * @author kaseyo
 */
@Entity
@Table(name = "Token")
public class TokenEntity extends AbstractEntity {
    public enum TokenStatus {
	    VALID,
	    REQUESTED,
	    EXPIRED,
	    TEMPORAL;
	}

	@Column(name = "token", nullable = false)
    private String token;
    
    @Column(name = "status", nullable = false)
    @Enumerated(EnumType.STRING)
    private TokenStatus status;
    
    public TokenEntity() {}
    
    public TokenEntity(String token, TokenStatus status){
        this.token = token;
        this.status = status;
    }
    
    public String getToken() {
        return this.token;
    }
    
    public TokenStatus getStatus() {
        return status;
    }
    
    @JsonIgnore
    public Boolean isValid() {
    	return this.status == TokenStatus.VALID;
    }
}
