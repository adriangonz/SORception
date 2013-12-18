/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.dao;

import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;

import com.sorception.jscrap.entities.UserEntity;
import java.util.List;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import org.springframework.stereotype.Repository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

/**
 *
 * @author kaseyo
 */
@Repository
@Transactional
public class UserDAO {
    @PersistenceContext(name = "entityManagerFactory")
    private EntityManager entityManager;
    
    public UserEntity addUser(UserEntity user) {
        this.entityManager.persist(user);
        return user;
    }
    
    @SuppressWarnings("unchecked")
    public List<UserEntity> getAllUsers() {
        return this.entityManager.createQuery("from UserEntity").getResultList();
    }
    
    public Boolean deleteUser(Long userId) {
        UserEntity user = (UserEntity) this.entityManager
                .getReference(UserEntity.class, userId);
        if (null != user) {
            this.entityManager.remove(user);
            return false;
        }
        return true;
    }
    
    public UserEntity getUser(Long userId) {
        return (UserEntity)this.entityManager.find(UserEntity.class, userId);
    }
    
    public UserEntity getUserByUsername(String username) {
        List<UserEntity> users = this.entityManager
                .createQuery("from UserEntity where username = :username")
                .setParameter("username", username)
                .getResultList();
        if(users.isEmpty())
            return null;
        else
            return users.get(0);
    }
}
