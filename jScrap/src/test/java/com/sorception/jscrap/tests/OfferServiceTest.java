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
import com.sorception.jscrap.entities.OfferLineEntity;
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
	public void OfferService_GetAll_ShouldReturnTwo() {
		List<OfferEntity> offers = offerService.getAllOffers();
		assertThat(offers.size(), is(2));
	}
	
	@Test
	public void OfferService_Get_ShouldReturnOne() {
		OfferEntity offer = offerService.getOfferById(1L);
		assertThat(offer.getLines().size(), is(1));
		assertThat(offer.getId(), is(1L));
		assertThat(offer.getOrder().getId(), is(1L));
	}
	
	@Test
	public void OfferService_Create_ShouldReturnOne() {
		OfferDTO offer = new OfferDTO();
		offer.lines = generateRandom();
		OfferEntity newOffer = offerService.addOffer(offer);
		
		assertThat(newOffer.getId(), is(notNullValue()));
		assertThat(newOffer.getLines().size(), is(3));
		assertThat(newOffer.getOrder().getId(), is(2L));
	}
	
	@Test
	public void OfferService_Update_ShouldModify() {
		List<OfferLineDTO> lines = generateRandom();
		lines.get(0).notes = "add";
		lines.get(1).notes = "add";
		lines.get(2).id = 2L;
		lines.get(2).notes = "modified";
		OfferDTO offer = new OfferDTO();
		offer.lines = lines;
		OfferEntity modifiedOffer = offerService.updateOffer(1L, offer);
		OfferLineEntity line = offerService.getOfferLine(2L);
		
		assertThat(modifiedOffer.getLines().size(), is(3));
		assertThat(line.getNotes(), is("modified"));
	}
	
	@Test
	public void OfferService_UpdateAndDelete_ShouldModify() {
		OfferService_Update_ShouldModify();
		List<OfferLineDTO> lines = generateRandom();
		lines.get(0).id = 2L;
		lines.get(0).status = "DELETE";
		lines.get(1).id = 5L;
		lines.get(2).id = 6L;
		OfferDTO offer = new OfferDTO();
		offer.lines = lines;
		OfferEntity modifiedOffer = offerService.updateOffer(1L, offer);
		
		assertThat(modifiedOffer.getLines().size(), is(2));
	}
	
	@Test
	public void OfferService_DeleteAllLines_ShouldModify() {
		List<OfferLineDTO> lines = new ArrayList<>();
		OfferLineDTO line = new OfferLineDTO();
		line.id = 2L;
		line.status = "DELETE";
		lines.add(line);
		OfferDTO offer = new OfferDTO();
		offer.lines = lines;
		OfferEntity modifiedOffer = offerService.updateOffer(1L, offer);
		
		assertThat(modifiedOffer, is(nullValue()));
		assertThat(offerService.getAllOffers().size(), is(1));
	}
	
	private List<OfferLineDTO> generateRandom() {
		List<OfferLineDTO> lines = new ArrayList<>();
		for(int i = 0; i < 3; i++) {
			OfferLineDTO line = new OfferLineDTO();
			line.date = new Date();
			line.notes = "new line test 1";
			line.price = 2.3;
			line.quantity = 2;
			line.orderLineId = i + 3L;
			lines.add(line);
		}
		return lines;
	}
}
