package com.sorception.jscrap.dao;

import java.util.List;

import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import com.sorception.jscrap.entities.TokenEntity;

@Repository
public interface ITokenDAO extends IGenericDAO<TokenEntity> {
	public TokenEntity findByStatus(TokenEntity.TokenStatus status);
	
	@Query("SELECT DISTINCT token "
			+ "FROM TokenEntity AS token "
			+ "WHERE token.status = ?1 "
				+ "OR token.status = ?2 "
			+ "ORDER BY token.created DESC")
	public List<TokenEntity> findByStatus(TokenEntity.TokenStatus status1, 
			TokenEntity.TokenStatus status2);
	
	@Modifying  
	@Query("UPDATE TokenEntity t "
			+ "SET t.status = 'EXPIRED' "
			+ "WHERE t.status = 'VALID'")
	public void invalidateTokens();
	
	@Query("SELECT t "
			+ "FROM TokenEntity AS t "
			+ "ORDER BY token.created DESC")
	public List<TokenEntity> findAllOrderByCreated();
}
