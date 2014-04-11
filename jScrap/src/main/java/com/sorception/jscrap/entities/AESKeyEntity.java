package com.sorception.jscrap.entities;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.EnumType;
import javax.persistence.Enumerated;
import javax.persistence.Lob;
import javax.persistence.Table;

@Entity
@Table(name="AESKey")
public class AESKeyEntity extends AbstractEntity {
	
	public enum AESKeyType {
		SG,
		SCRAP
	}
	
	@Column(name = "aesIv", nullable = false)
	@Lob
	private byte[] aesIv;
	
	@Column(name = "aesKey", nullable = false)
	@Lob
	private byte[] aesKey;
	
	@Column(name = "type", nullable = false)
	@Enumerated(EnumType.STRING)
	private AESKeyType type;
	
	public AESKeyEntity(byte[] iv, byte[] key, AESKeyType type) {
		this.aesIv = iv;
		this.aesKey = key;
		this.type = type;
	}
	
	public byte[] getKey() {
		return aesKey;
	}
	
	public byte[] getIv() {
		return aesIv;
	}
}
