package com.sorception.jscrap.error;

public class BusinessException extends RuntimeException {
	
	final private String details;
    
    public BusinessException(String details) {
        this.details = details;
    }
    
    @Override
    public String getMessage() {
        String message = "{\"message\": \"Business exception\"";
        message += ",\"details\": \"" + this.details + "\"}";
        return message;
    }
}
