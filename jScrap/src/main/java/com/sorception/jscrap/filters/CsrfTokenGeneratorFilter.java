package com.sorception.jscrap.filters;

import java.io.IOException;

import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.security.web.csrf.CsrfToken;
import org.springframework.web.filter.OncePerRequestFilter;

public class CsrfTokenGeneratorFilter extends OncePerRequestFilter {
	final Logger logger = LoggerFactory.getLogger(getClass());
	
	@Override
    protected void doFilterInternal(HttpServletRequest request, HttpServletResponse response, FilterChain filterChain) throws ServletException, IOException {
        CsrfToken token = (CsrfToken) request.getAttribute("_csrf");

        // Spring Security will allow the Token to be included in this header name
        response.setHeader("X-CSRF-HEADER", token.getHeaderName());
 
        // Spring Security will allow the token to be included in this parameter name
        response.setHeader("X-CSRF-PARAM", token.getParameterName());
 
        // this is the value of the token to be included as either a header or an HTTP parameter
        response.setHeader("X-CSRF-TOKEN", token.getToken());
 
        logger.info("Setteo los attrs de header: " + token.getHeaderName() + ", param: " + token.getParameterName() + ", token: " + token.getToken());
        
        filterChain.doFilter(request, response);
    }
}
