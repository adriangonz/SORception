package com.sorception.jscrap.tests;

import static org.hamcrest.Matchers.*;
import static org.junit.Assert.*;

import java.util.List;

import org.junit.Before;
import org.junit.Test;

import com.sorception.jscrap.entities.LogEntity;
import com.sorception.jscrap.services.LogService;

public class LogTest extends BaseTest {
	
	private LogService logService;
	
	@Before
    public void setup() {
        // workaround for autowiring problem
        logService = (LogService)applicationContext.getBean("logService");
        loginUser("admin");
    }
	
	@Test
	public void getLogs_ShouldNotBeEmpty() {
		List<LogEntity> logs = logService.getLogs();
		assertThat(logs, is(not(empty())));
	}
}
