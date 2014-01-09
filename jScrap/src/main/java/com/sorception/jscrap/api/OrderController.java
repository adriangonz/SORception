package com.sorception.jscrap.api;

import java.util.ArrayList;
import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.ResponseStatus;
import org.springframework.http.HttpStatus;

import com.sorception.jscrap.entities.OrderEntity;
import com.sorception.jscrap.entities.OrderLineEntity;
import com.sorception.jscrap.services.OrderService;

class OrderLineDTO {
	public String sgId;
	public String description;
	public Integer quantity;
	
	public OrderLineEntity toOrderLineEntity() {
		return new OrderLineEntity(this.sgId, 
				this.description, this.quantity);
	}
}

class OrderParamsDTO {
	public String sgId;
	public List<OrderLineDTO> lines;
	
	public List<OrderLineEntity> getOrderLinesList() {
		List<OrderLineEntity> list = new ArrayList<>();
		for(OrderLineDTO line : lines) {
			list.add(line.toOrderLineEntity());
		}
		return list;
	}
}

@Controller
@RequestMapping("/api/order")
public class OrderController {
	
	final static Logger logger = LoggerFactory.getLogger(OrderController.class);
	
	@Autowired
	private OrderService orderService;
	
	@RequestMapping(value = "", method = RequestMethod.GET)
	@ResponseBody
	public List<OrderEntity> getOrders() {
		return orderService.getAllOrders();
	}
	
	@RequestMapping(value = "", method = RequestMethod.POST)
	@ResponseBody
	@ResponseStatus(HttpStatus.CREATED)
	public OrderEntity addOrder(@RequestBody OrderParamsDTO order) {
		return orderService.addOrder(order.sgId, 
				order.getOrderLinesList());
	}
}
