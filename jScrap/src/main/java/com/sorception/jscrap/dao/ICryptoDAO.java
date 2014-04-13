package com.sorception.jscrap.dao;

import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import com.sorception.jscrap.entities.AESKeyEntity;
import com.sorception.jscrap.entities.AESKeyEntity.AESKeyType;

public interface ICryptoDAO extends IGenericDAO<AESKeyEntity> {
	
	@Query("SELECT DISTINCT k "
			+ "FROM AESKeyEntity AS k "
			+ "WHERE k.type = :type "
			+ "ORDER BY k.created DESC")
	public List<AESKeyEntity> findByTypeOrderByCreated(@Param("type") AESKeyType type);
}
