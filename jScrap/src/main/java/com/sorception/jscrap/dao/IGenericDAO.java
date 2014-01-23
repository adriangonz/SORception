package com.sorception.jscrap.dao;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.JpaSpecificationExecutor;
import org.springframework.stereotype.Repository;

import com.sorception.jscrap.entities.AbstractEntity;

public interface IGenericDAO<T extends AbstractEntity> extends JpaRepository<T, Long>, JpaSpecificationExecutor<T> {

}
