package com.sorception.jscrap.services;

import java.security.SecureRandom;
import java.util.List;

import javax.crypto.Cipher;
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
	
	public void saveSGKey(byte[] key, byte[] iv) {
		logger.info("Saving SG AES Key and IV.");
		AESKeyEntity aes = new AESKeyEntity(iv, key, AESKeyType.SG);
		this.create(aes);
	}
	
	public AESKeyEntity getScrapKey() {
		logger.info("Retrieving jScrap AES Key and IV.");
		return getKey(AESKeyType.SCRAP);
	}
	
	public AESKeyEntity getSGKey() {
		logger.info("Retrieving SG AES Key and IV.");
		return getKey(AESKeyType.SG);
	}
	
	public String decrypt(byte[] encrypted, AESKeyEntity keyEntity) {
		try {
			Cipher aes = getCipher(keyEntity, Cipher.DECRYPT_MODE);
			byte[] original = aes.doFinal(encrypted);
			String clear = new String(original);
			logger.info("Decrypted " + clear);
			return clear;
		} catch (Exception e) {
			throw new RuntimeException("Decrypting failure");
		}
	}
	
	public byte[] encrypt(String clearText, AESKeyEntity keyEntity) {
		try {
			logger.info("Encrypting " + clearText);
			Cipher aes = getCipher(keyEntity, Cipher.ENCRYPT_MODE);
			return aes.doFinal(clearText.getBytes());
		} catch (Exception ex) {
			throw new RuntimeException("Encrypting failure");
		}
	}
	
	private Cipher getCipher(AESKeyEntity key, int opmode) throws Exception {
		Cipher aes = Cipher.getInstance("AES/CBC/PKCS5Padding");
		aes.init(opmode, key.toSecretKeySpec(), key.toIvParameterSpec());
		return aes;
	}
	
	private AESKeyEntity getKey(AESKeyType type) {
		List<AESKeyEntity> keys = getCustomDao().findByTypeOrderByCreated(type);
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
