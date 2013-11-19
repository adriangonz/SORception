/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.dao;

import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;

import com.sorception.jscrap.models.UserEntity;
import java.io.Serializable;
import java.util.List;
import org.springframework.stereotype.Service;

/**
 *
 * @author kaseyo
 */
@Service
public class UserDAO {
    @Autowired
    private SessionFactory sessionFactory;
    
    public Long addUser(UserEntity user) {
        return (Long)this.sessionFactory.getCurrentSession().save(user);
    }
    
    @SuppressWarnings("unchecked")
    public List<UserEntity> getAllUsers() {
        return this.sessionFactory
                .getCurrentSession().createQuery("from UserEntity").list();
    }
    
    public void deleteUser(Long userId) {
        UserEntity user = (UserEntity) this.sessionFactory
                .getCurrentSession().load(UserEntity.class, userId);
        if (null != user) {
            this.sessionFactory.getCurrentSession().delete(user);
        }
    }
}
