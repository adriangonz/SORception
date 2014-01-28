package com.sorception.jscrap.tests;

import static org.hamcrest.Matchers.*;
import static org.junit.Assert.*;

import java.util.ArrayList;
import java.util.List;

import org.junit.Before;
import org.junit.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;

import com.github.springtestdbunit.annotation.DatabaseSetup;
import com.sorception.jscrap.entities.OrderEntity;
import com.sorception.jscrap.entities.OrderLineEntity;
import com.sorception.jscrap.error.ResourceNotFoundException;
import com.sorception.jscrap.services.OfferService;
import com.sorception.jscrap.services.OrderService;

@DatabaseSetup("classpath:orderDataset.xml")
public class OrderServiceTest extends BaseTest {
	
	@InjectMocks
	OrderService orderService;
	
	@Mock
	OfferService offerService;
	
	@Before
	public void setup() {
		orderService = (OrderService)applicationContext.getBean("orderService");
	}
	
	@Test
	public void OrderService_ShouldNotBeNull() {
		assertThat(orderService, is(notNullValue()));
	}
	
	@Test
	public void OrderService_addOrder_ShouldNotBeNull() {
		List<OrderLineEntity> orderLines = new ArrayList<>();
		for(int i = 0; i < 2; i++) {
			orderLines.add(new OrderLineEntity(
					Integer.toString(i + 30),
					"Test orderline",
					2));
		}
		OrderEntity order = orderService.addOrder("30", orderLines);
		assertThat(order, is(notNullValue()));
		assertThat(order.getId(), is(notNullValue()));
		assertThat(order.getLines().size(), is(2));
	}
	
	@Test
	public void OrderService_getAll_ShouldReturnOne() {
		List<OrderEntity> orders = orderService.getAllOrders();
		assertThat(orders.size(), is(1));
	}
	
	@Test(expected = ResourceNotFoundException.class)
	public void OrderService_delete_ShouldReturnEmpty() {
		OrderEntity order = orderService.getOrderById(1L);
		orderService.deleteOrder(order);
		List<OrderEntity> orders = orderService.getAllOrders();
		assertThat(orders.size(), is(0));
		orderService.getOrderById(1L);
	}
	
	@Test(expected = ResourceNotFoundException.class)
	public void OrderService_close_ShouldReturnEmpty() {
		OrderEntity order = orderService.getOrderById(1L);
		orderService.closeOrder(order);
		List<OrderEntity> orders = orderService.getAllOrders();
		assertThat(orders.size(), is(0));
		orderService.getOrderById(1L);
	}
}
