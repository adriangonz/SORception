package controllers;

import org.junit.Before;
import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.MockitoAnnotations;
import org.springframework.test.web.servlet.MockMvc;

import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultHandlers.print;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.content;
import static org.springframework.test.web.servlet.setup.MockMvcBuilders.standaloneSetup;

public class IndexIntegrationTest {
	private static final String RESPONSE_BODY = "Ola ke ase!";
	
	MockMvc mockMvc;
	
	@InjectMocks
	IndexController controller = new IndexController();
	
	@Before
	public void setup() {
		MockitoAnnotations.initMocks(this);
		mockMvc = standaloneSetup(controller).build();
	}
	
	@Test
	public void thatTextReturned() throws Exception {
		mockMvc.perform(get("/"))
			.andDo(print())
			.andExpect(content().string(RESPONSE_BODY));
	}
}
