package com.sorception.jscrap.tests;

import static org.hamcrest.Matchers.*;
import static org.junit.Assert.*;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import org.junit.Before;
import org.junit.Test;

import com.github.springtestdbunit.annotation.DatabaseSetup;
import com.sorception.jscrap.dto.OfferDTO;
import com.sorception.jscrap.dto.OfferLineDTO;
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
	
	@Test
	public void OfferService_Get_ShouldReturnOne() {
		OfferEntity offer = offerService.getOfferById(1L);
		assertThat(offer.getLines().size(), is(2));
		assertThat(offer.getId(), is(1L));
		assertThat(offer.getOrder().getId(), is(1L));
	}
	
	@Test
	public void OfferService_Create_ShouldReturnOne() {
		List<OfferLineDTO> lines = new ArrayList<>();
		for(int i = 0; i < 2; i++) {
			OfferLineDTO line = new OfferLineDTO();
			line.date = new Date();
			line.notes = "new line test 1";
			line.price = 2.3;
			line.quantity = 2;
			line.orderLineId = i + 3L;
			lines.add(line);
		}
		OfferDTO offer = new OfferDTO();
		offer.lines = lines;
		OfferEntity newOffer = offerService.addOffer(offer);
		
		assertThat(newOffer.getId(), is(notNullValue()));
		assertThat(newOffer.getLines().size(), is(2));
		assertThat(newOffer.getOrder().getId(), is(2L));
	}
}
