package com.sorception.jscrap.entities;

import java.util.Date;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class LogEntity {
	
	final Logger logger = LoggerFactory.getLogger(getClass());
	
	private String userName;
	private String message;
	private Date date;
	
	public LogEntity(String userName, String message, Date date) {
		this.userName = userName;
		this.message = message;
		this.date = date;
	}
	
	public String getUsername() {
		return userName;
	}
	
	public String getMessage() {
		return message;
	}
	
	public Date getDate() {
		return date;
	}
}
