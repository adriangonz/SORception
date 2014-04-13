package com.sorception.jscrap.api;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;

import com.sorception.jscrap.entities.LogEntity;
import com.sorception.jscrap.services.LogService;

@Controller
@RequestMapping("/api/log")
public class LogController {

	@Autowired
    private LogService logService;
    
    @RequestMapping(value="", method=RequestMethod.GET)
    @ResponseBody
    public List<LogEntity> getLogs() {
    	return logService.getLogs();
    }
}
