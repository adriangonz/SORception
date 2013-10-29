package controllers;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;

@Controller
@RequestMapping("/")
public class IndexController {
	private static final Logger LOG = LoggerFactory.getLogger(IndexController.class);
	
	@RequestMapping(value = {"","/"}, method=RequestMethod.GET)
	@ResponseBody
	public String get() {
		LOG.debug("This should return some text...");
		return "Ola ke ase!";
	}
}
