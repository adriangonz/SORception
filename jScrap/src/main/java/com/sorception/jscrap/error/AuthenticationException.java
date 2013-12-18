/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.error;

/**
 *
 * @author kaseyo
 */
public class AuthenticationException extends RuntimeException {
    final private String details;
    
    public AuthenticationException(String details) {
        this.details = details;
    }
    
    @Override
    public String getMessage() {
        String message = "{\"message\": \"Forbidden access\"";
        message += ",\"details\": \"" + this.details + "\"}";
        return message;
    }
}
