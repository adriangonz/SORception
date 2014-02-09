package com.sorception.jscrap.services;

import org.springframework.data.domain.AuditorAware;
import org.springframework.stereotype.Service;

import com.sorception.jscrap.entities.UserEntity;

@Service
public class AuditorService implements AuditorAware<UserEntity> {

	@Override
	public UserEntity getCurrentAuditor() {
		return null;
	}

}
