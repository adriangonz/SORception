package com.sorception.jscrap.services;

import java.security.SecureRandom;
import java.util.List;

import javax.crypto.KeyGenerator;
import javax.crypto.SecretKey;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.sorception.jscrap.dao.ICryptoDAO;
import com.sorception.jscrap.dao.IGenericDAO;
import com.sorception.jscrap.entities.AESKeyEntity;
import com.sorception.jscrap.entities.AESKeyEntity.AESKeyType;
import com.sorception.jscrap.error.ResourceNotFoundException;

@Service
@Transactional
public class CryptoService extends AbstractService<AESKeyEntity> {
	
	public CryptoService() {
		super(AESKeyEntity.class);
	}

	@Autowired
    private ICryptoDAO dao;

	public AESKeyEntity generateAES()
	{
		logger.info("Generating random AES Key and IV.");
	    byte[] key = generateAESKey();
	    byte[] iv = generateAESIv();
	    AESKeyEntity aes = new AESKeyEntity(iv, key, AESKeyType.SCRAP);
	    this.create(aes);
	    return aes;
	}
	
	public AESKeyEntity getScrapKey() {
		return getKey(AESKeyType.SCRAP);
	}
	
	public AESKeyEntity getSGKey() {
		return getKey(AESKeyType.SG);
	}
	
	private AESKeyEntity getKey(AESKeyType type) {
		List<AESKeyEntity> keys = getCustomDao().findByTypeOrderByCreatedAsc(type);
		if(keys.isEmpty())
			throw new ResourceNotFoundException(type.toString() + " key not found");
		return keys.get(0);
	}
	
	private byte[] generateAESKey() {
		KeyGenerator keyGenerator;
		try {
			keyGenerator = KeyGenerator.getInstance("AES");
			keyGenerator.init(256);
		    SecretKey secretKey = keyGenerator.generateKey();
		    return secretKey.getEncoded();
		} catch(Exception ex) {return null;}
	}
	
	private byte[] generateAESIv() {
		SecureRandom random = new SecureRandom();
		byte[] ivBytes = new byte[16];
		random.nextBytes(ivBytes);
		return ivBytes;
	}

	@Override
	protected IGenericDAO<AESKeyEntity> getDao() {
		// TODO Auto-generated method stub
		return dao;
	}
	
	protected ICryptoDAO getCustomDao() {
		// TODO Auto-generated method stub
		return dao;
	}
}
