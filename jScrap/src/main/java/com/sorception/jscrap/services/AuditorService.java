package com.sorception.jscrap.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.AuditorAware;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Service;

import com.sorception.jscrap.entities.CustomUserDetails;
import com.sorception.jscrap.entities.UserEntity;

@Service
public class AuditorService implements AuditorAware<UserEntity> {

	@Autowired
	UserService userService;
	
	@Override
	public UserEntity getCurrentAuditor() {
		Authentication authentication = SecurityContextHolder.getContext().getAuthentication();

	    if (authentication == null || !authentication.isAuthenticated()) {
	      return null;
	    }
	    
	    CustomUserDetails userDetails = (CustomUserDetails) authentication.getPrincipal();
	    UserEntity user = userDetails.getUser();
	    return user;
	}

}
