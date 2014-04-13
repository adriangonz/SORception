package com.sorception.jscrap.dao;

import java.util.List;

import org.springframework.data.jpa.repository.Query;

import com.sorception.jscrap.entities.UserEntity;

public interface ILogDAO extends IGenericDAO<UserEntity>{
	
	@Query(value = "SELECT p.mapped_value, e.formatted_message, e.timestmp "
			+ "FROM logging_event e LEFT JOIN logging_event_property p "
				+ "ON e.event_id = p.event_id "
					+ "AND p.mapped_key = 'userName' "
			+ "ORDER BY e.timestmp DESC", nativeQuery = true)
	List<Object[]> getLogEntries();
}
