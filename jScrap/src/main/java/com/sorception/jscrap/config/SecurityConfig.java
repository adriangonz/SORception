/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.config;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.http.HttpMethod;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.dao.DaoAuthenticationProvider;
import org.springframework.security.authentication.dao.ReflectionSaltSource;
import org.springframework.security.authentication.encoding.ShaPasswordEncoder;
import org.springframework.security.config.annotation.authentication.builders.AuthenticationManagerBuilder;
import org.springframework.security.config.annotation.method.configuration.EnableGlobalMethodSecurity;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter;
import org.springframework.security.config.http.SessionCreationPolicy;
import org.springframework.security.core.userdetails.UserDetailsService;

/**
 *
 * @author kaseyo
 */
@Configuration
@EnableWebSecurity
@EnableGlobalMethodSecurity(prePostEnabled = true)
public class SecurityConfig extends WebSecurityConfigurerAdapter{
    @Autowired
    UserDetailsService userDetailsService;
    
    @Autowired
    @Override
    public void configure(AuthenticationManagerBuilder auth) throws Exception {
    	auth
    		.authenticationProvider(daoAuthenticationProvider());
    }
    
    @Bean
    public DaoAuthenticationProvider daoAuthenticationProvider() {
    	DaoAuthenticationProvider daoAuthenticationProvider = new DaoAuthenticationProvider();
    	daoAuthenticationProvider.setPasswordEncoder(passwordEncoder());
    	daoAuthenticationProvider.setUserDetailsService(userDetailsService);
    	daoAuthenticationProvider.setSaltSource(saltSource());
    	return daoAuthenticationProvider;
    }
    
    @Bean
    public ShaPasswordEncoder passwordEncoder() {
    	return new ShaPasswordEncoder(256);
    }
    
    @Bean
    public ReflectionSaltSource saltSource() {
    	ReflectionSaltSource saltSource = new ReflectionSaltSource();
    	saltSource.setUserPropertyToUse("username");
    	return saltSource;
    }
    
    @Bean
    @Override
    public AuthenticationManager authenticationManagerBean() throws Exception {
        return super.authenticationManagerBean();
    }
    
    @Override
    protected void configure(HttpSecurity http) throws Exception {
        http
          .sessionManagement()
            .sessionCreationPolicy(SessionCreationPolicy.STATELESS)
            .enableSessionUrlRewriting(false)
            .and()
          .authorizeRequests()
          	.antMatchers(HttpMethod.POST, "/api/user/authenticate").permitAll()
            .antMatchers(HttpMethod.POST, "/api/user").hasRole("ADMIN")
            .antMatchers(HttpMethod.DELETE, "/api/user/[0-9]+").hasRole("ADMIN")
            .antMatchers(HttpMethod.POST, "/api/token").hasRole("ADMIN")
            .antMatchers("/api/user", "/api/order/**", "/api/offer/**", 
            				"/api/accepted/**", "/api/token/**",
            				"/api/settings/**").hasRole("USER")
            .anyRequest().permitAll()
            .and()
           .csrf().disable()
          .requiresChannel()
          	.anyRequest().requiresSecure()
          	.and()
          .httpBasic();
    }
}
