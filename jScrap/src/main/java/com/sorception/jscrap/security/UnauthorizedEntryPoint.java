/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.security;

import java.io.IOException;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.springframework.security.core.AuthenticationException;
import org.springframework.security.web.AuthenticationEntryPoint;

public class UnauthorizedEntryPoint implements AuthenticationEntryPoint {

        @Override
        public void commence(HttpServletRequest request, HttpServletResponse response, AuthenticationException authException)
                        throws IOException, ServletException {

                response.sendError(
                                HttpServletResponse.SC_UNAUTHORIZED,
                                "Unauthorized: Authentication token was either missing or invalid.");
        }
}
