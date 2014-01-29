/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.dao;

import org.springframework.stereotype.Repository;

import com.sorception.jscrap.entities.UserEntity;

/**
 *
 * @author kaseyo
 */

@Repository
public interface IUserDAO extends IGenericDAO<UserEntity>{
	UserEntity findByUsername(String username);
}
