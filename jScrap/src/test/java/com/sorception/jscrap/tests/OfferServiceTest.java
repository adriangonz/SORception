package com.sorception.jscrap.tests;

import static org.hamcrest.Matchers.*;
import static org.junit.Assert.*;

import java.util.List;

import org.junit.Before;
import org.junit.Test;

import com.github.springtestdbunit.annotation.DatabaseSetup;
import com.sorception.jscrap.entities.OfferEntity;
import com.sorception.jscrap.services.OfferService;

@DatabaseSetup("classpath:offerDataset.xml")
public class OfferServiceTest extends BaseTest {
	
	OfferService offerService;
	
	@Before
	public void setup() {
		offerService = (OfferService)applicationContext.getBean("offerService");
	}
	
	@Test
	public void OfferService_ShouldNotBeNull() {
		assertThat(offerService, is(notNullValue()));
	}
	
	@Test
	public void OfferService_GetAll_ShouldReturnOne() {
		List<OfferEntity> offers = offerService.getAllOffers();
		assertThat(offers.size(), is(1));
	}
}
