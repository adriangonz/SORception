/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.sorception.jscrap.models;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import javax.persistence.Table;

/**
 *
 * @author kaseyo
 */
@Entity
@Table(name="User")
public class UserEntity {
    @Id
    @Column(name="Id")
    @GeneratedValue
    private Long _id;
    
    @Column(name="Name")
    private String _name;
}
