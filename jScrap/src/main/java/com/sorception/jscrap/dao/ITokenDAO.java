package com.sorception.jscrap.dao;

import java.util.List;

import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.entities.TokenEntity;

@Transactional
@Repository
public interface ITokenDAO extends IGenericDAO<TokenEntity> {
	public TokenEntity findByStatus(TokenEntity.TokenStatus status);
	
	@Query("select token "
			+ "from TokenEntity token "
			+ "where token.status = ?1 or token.status = ?2 "
			+ "order by token.created desc")
	public List<TokenEntity> findByStatus(TokenEntity.TokenStatus status1, 
			TokenEntity.TokenStatus status2);
	
	@Modifying  
	@Query("UPDATE TokenEntity t SET t.status = 'EXPIRED' WHERE t.status = 'VALID'")
	public void invalidateTokens();
}
