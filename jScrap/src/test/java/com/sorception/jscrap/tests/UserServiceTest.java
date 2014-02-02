package com.sorception.jscrap.tests;

import static org.hamcrest.Matchers.*;
import static org.junit.Assert.*;

import java.util.List;

import org.junit.Before;
import org.junit.Test;

import com.github.springtestdbunit.annotation.DatabaseSetup;
import com.sorception.jscrap.dto.UserInfoDTO;
import com.sorception.jscrap.entities.UserEntity;
import com.sorception.jscrap.services.UserService;

@DatabaseSetup("classpath:userDataset.xml")
public class UserServiceTest extends BaseTest {
	UserService userService;
	
	@Before
    public void setup() {
        // workaround for autowiring problem
        userService = (UserService)applicationContext.getBean("userService");
    }
	
	@Test
	public void UserService_ShouldNotBeNull() {
		assertThat(userService, is(notNullValue()));
	}
	
	@Test
	public void UserService_GetAll_ShouldReturnOneElement() {
		List<UserEntity> users = userService.getAllUsers();
		assertThat(users.size(), is(2));
	}
	
	@Test
	public void UserService_Register_ShouldReturnUser() {
		UserInfoDTO userInfo = new UserInfoDTO();
		userInfo.username = "test";
		userInfo.name = "Test user";
		UserEntity user = userService.addUser(userInfo.username, userInfo.name);
		assertThat(user.getName(), is(userInfo.name));
		assertThat(user.getId(), is(notNullValue()));
	}
	
	@Test
	public void UserService_Get_ShouldReturnUserAdmin() {
		UserEntity admin = userService.getUser(1L);
		assertThat(admin.getUsername(), is("admin"));
		assertThat(admin.getId(), is(1L));
	}
	
	@Test
	public void UserService_Delete_ShouldReturnEmptyUser() {
		userService.delete(2);
		List<UserEntity> users = userService.getAllUsers();
		assertThat(users.size(), is(1)); // Only admin
	}
}
