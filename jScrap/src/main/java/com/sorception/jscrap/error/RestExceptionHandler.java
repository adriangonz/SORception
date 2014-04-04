package com.sorception.jscrap.error;

import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.security.authentication.AuthenticationCredentialsNotFoundException;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.context.request.WebRequest;
import org.springframework.web.servlet.mvc.method.annotation.ResponseEntityExceptionHandler;

import com.google.common.base.Throwables;

/**
 * General error handler for the application.
 */
@ControllerAdvice
public class RestExceptionHandler extends ResponseEntityExceptionHandler {

	/**
	 * Handle exceptions thrown by handlers.
	 */
        @ExceptionHandler(value = {
            ResourceNotFoundException.class, 
            ServiceUnavailableException.class, 
            AuthenticationException.class,
            BusinessException.class})
        @ResponseBody
        public ResponseEntity<Object> notFoundException(RuntimeException ex, WebRequest request) {
            HttpHeaders httpHeaders = new HttpHeaders();
            httpHeaders.setContentType(MediaType.APPLICATION_JSON);
            String message = getJsonInfo(ex);
            return this.handleExceptionInternal(
                    ex, message, httpHeaders, HttpStatus.NOT_FOUND, request);
        }
        
        @ExceptionHandler(Exception.class)
        @ResponseBody
        public ResponseEntity<Object> generalException(Exception ex, WebRequest request) {
        	HttpStatus status = HttpStatus.INTERNAL_SERVER_ERROR;
        	if(ex instanceof AuthenticationCredentialsNotFoundException)
        		status = HttpStatus.UNAUTHORIZED;
        	else if(ex instanceof AccessDeniedException)
        		status = HttpStatus.FORBIDDEN;
            HttpHeaders httpHeaders = new HttpHeaders();
            httpHeaders.setContentType(MediaType.APPLICATION_JSON);
            String message = getJsonInfo(ex);
            return this.handleExceptionInternal(
                    ex, message, httpHeaders, status, request);
        }
        
        private String getJsonInfo(Exception ex) {
        	String message = "{" +
            		"\"stacktrace\": \"" + Throwables.getStackTraceAsString(ex) + "\"" + 
            		"\"message\": \"" + ex.getMessage() + "\"}";
        	return message;
        }
}