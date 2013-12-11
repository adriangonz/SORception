/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.entities;

import java.util.List;

/**
 *
 * @author kaseyo
 */
public class SettingsEntity {
    // Dummy entity which holds information.
    // It is not persisted in the database,
    // because it is loaded from jscrap.properties and
    // other entities
    
    final private String _name;

    private List<TokenEntity> _tokenList = null;
    
    private TokenEntity _validToken = null; 
    
    public SettingsEntity(String _name) {
        this._name = _name;
    }

    public String getName() {
        return _name;
    }
    
    public List<TokenEntity> getTokenList() {
        return _tokenList;
    }

    public TokenEntity getValidToken() {
        return _validToken;
    }
    
    public void setTokenList(List<TokenEntity> tokenList) {
        this._tokenList = tokenList;
    }
    
    public void setValidToken(TokenEntity validToken) {
        this._validToken = validToken;
    }
}
