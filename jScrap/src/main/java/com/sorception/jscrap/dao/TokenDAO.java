/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.dao;

import com.sorception.jscrap.entities.TokenEntity;
import java.util.List;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

/**
 *
 * @author kaseyo
 */
@Service
public class TokenDAO {
    @Autowired
    SessionFactory sessionFactory;
    
    public Long save(TokenEntity tokenEntity) {
        if(tokenEntity.getStatus() == TokenEntity.TokenStatus.VALID) {
            // Set to expired any previous valid token
            sessionFactory
                    .getCurrentSession()
                    .createQuery("UPDATE TokenEntity SET Status = 'EXPIRED' "
                            + "WHERE Status = 'VALID'")
                    .executeUpdate();
        }
        
        // Save and return Id
        return (Long) sessionFactory
                        .getCurrentSession()
                        .save(tokenEntity);
    }
    
    public List<TokenEntity> list() {
        return sessionFactory
                .getCurrentSession()
                .createQuery("FROM TokenEntity")
                .list();
    }
    
    public TokenEntity getValid() {
        // Get Valid tokens (there should only be one)
        List<TokenEntity> tokenList = sessionFactory
                                        .getCurrentSession()
                                        .createQuery("FROM TokenEntity WHERE Status = 'VALID'")
                                        .list();
        // Return null or first token
        if(tokenList.isEmpty())
            return null;
        else
            return tokenList.get(0);
    }
}
