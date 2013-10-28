package controllers.api;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;

@Controller
@RequestMapping("/api/settings")
public class SettingsController {
	
	@RequestMapping(value = {"","/"}, method=RequestMethod.GET)
	@ResponseBody
	public String get() {
		return "<h1>Prueba</h1>";
	}
}
