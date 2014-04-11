package com.sorception.jscrap.dao;

import java.util.List;

import com.sorception.jscrap.entities.AESKeyEntity;
import com.sorception.jscrap.entities.AESKeyEntity.AESKeyType;

public interface ICryptoDAO extends IGenericDAO<AESKeyEntity> {

	public List<AESKeyEntity> findByTypeOrderByCreatedAsc(AESKeyType type);
}
