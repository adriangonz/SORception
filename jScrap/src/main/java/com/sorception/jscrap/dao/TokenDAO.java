/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.dao;

import com.sorception.jscrap.entities.TokenEntity;
import java.util.List;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

/**
 *
 * @author kaseyo
 */
@Repository
@Transactional
public class TokenDAO {
    @PersistenceContext(name = "entityManagerFactory")
    EntityManager entityManager;
    
    public TokenEntity save(TokenEntity tokenEntity) {
        if(tokenEntity.getStatus() == TokenEntity.TokenStatus.VALID
                || tokenEntity.getStatus() == TokenEntity.TokenStatus.REQUESTED) {
            // Set to expired any previous valid token
            this.entityManager
                    .createQuery("UPDATE TokenEntity SET status = 'EXPIRED' "
                            + "WHERE status = 'VALID'")
                    .executeUpdate();
        }
        
        // Save and return Id
        this.entityManager.persist(tokenEntity);
        return tokenEntity;
    }
    
    public List<TokenEntity> list() {
        return this.entityManager
                .createQuery("FROM TokenEntity ORDER BY creationDate DESC")
                .getResultList();
    }
    
    public TokenEntity getValid() {
        // Get Valid tokens (there should only be one)
        List<TokenEntity> tokenList = this.entityManager
                                        .createQuery("FROM TokenEntity WHERE status = 'VALID'")
                                        .getResultList();
        // Return null or first token
        if(tokenList.isEmpty())
            return null;
        else
            return tokenList.get(0);
    }
    
    public TokenEntity getRequestOrTemporal() {
        // Get last requested token
        List<TokenEntity> tokenList = this.entityManager
                                        .createQuery("FROM TokenEntity WHERE status = 'REQUESTED'"
                                                + " OR status = 'TEMPORAL'"
                                                + " ORDER BY creationDate DESC")
                                        .getResultList();
        if(tokenList.isEmpty())
            return null;
        else
            return tokenList.get(0);
    }
}
