package com.sorception.jscrap.tests;

import static org.hamcrest.Matchers.*;
import static org.junit.Assert.*;

import org.junit.Before;
import org.junit.Test;

import com.sorception.jscrap.entities.AESKeyEntity;
import com.sorception.jscrap.services.CryptoService;

public class CryptoTest extends BaseTest {

	private CryptoService cryptoService;
	
	@Before
	public void setup() {
		cryptoService = (CryptoService)applicationContext.getBean("cryptoService");
	}
	
	@Test
	public void generateAES_ShouldReturnObject() {
		AESKeyEntity aesKey = cryptoService.generateAES();
		assertThat(aesKey.getKey().length, is(not(0)));
		assertThat(aesKey.getIv().length, is(not(0)));
	}
	
	@Test
	public void retrieveAES_ShouldReturnObject() {
		AESKeyEntity aesKey = cryptoService.generateAES();
		AESKeyEntity retrievedKey = cryptoService.getScrapKey();
		assertEquals(aesKey.getIv(), retrievedKey.getIv());
		assertEquals(aesKey.getKey(), retrievedKey.getKey());
	}
	
	@Test
	public void retrieveAES_TwoCreated_ShouldReturnObject() {
		cryptoService.generateAES();
		AESKeyEntity aesKey = cryptoService.generateAES();
		AESKeyEntity retrievedKey = cryptoService.getScrapKey();
		assertEquals(aesKey.getIv(), retrievedKey.getIv());
		assertEquals(aesKey.getKey(), retrievedKey.getKey());
	}
}
