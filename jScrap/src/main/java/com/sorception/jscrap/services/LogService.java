package com.sorception.jscrap.services;

import java.math.BigInteger;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.sorception.jscrap.dao.ILogDAO;
import com.sorception.jscrap.entities.LogEntity;

@Service
public class LogService {
	
	@Autowired
	private ILogDAO logsDao;
	
	public List<LogEntity> getLogs() {
		List<LogEntity> logs = new ArrayList<>();
		List<Object[]> rawLogs = logsDao.getLogEntries();
		for (Object[] rawLog : rawLogs) {
			String userName = "system";
			if (rawLog[0] != null)
				userName = (String) rawLog[0];
			String message = (String) rawLog[1];
			BigInteger rawTimestamp = (BigInteger) rawLog[2];
			Date timestamp = new Date(rawTimestamp.longValue());
			logs.add(new LogEntity(userName, message, timestamp));
		}
		
		return logs;
	}
}
