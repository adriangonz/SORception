package com.sorception.jscrap.error;

import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.context.request.WebRequest;

import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.servlet.mvc.method.annotation.ResponseEntityExceptionHandler;

/**
 * General error handler for the application.
 */
@ControllerAdvice
class RestExceptionHandler extends ResponseEntityExceptionHandler {

	/**
	 * Handle exceptions thrown by handlers.
	 */
        @ExceptionHandler(value = ResourceNotFoundException.class)
        public ResponseEntity<Object> notFoundException(RuntimeException ex, WebRequest request) {
            String message = "{\"message\": \"Resource not found\"}";
            HttpHeaders httpHeaders = new HttpHeaders();
            httpHeaders.setContentType(MediaType.APPLICATION_JSON);
            return this.handleExceptionInternal(
                    ex, message, new HttpHeaders(), HttpStatus.NOT_FOUND, request);
        }
}