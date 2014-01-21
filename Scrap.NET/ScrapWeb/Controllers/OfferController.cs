using ScrapWeb.DTO;
using ScrapWeb.Entities;
using ScrapWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScrapWeb.Controllers
{
    public class OfferController : ApiController
    {
        private OfferService offerService;

        OfferController()
        {
            offerService = new OfferService();
        }

        // GET api/<controller>
        public IEnumerable<OfferEntity> Get()
        {
            return offerService.getAll();
        }

        // GET api/<controller>/5
        public OfferEntity Get(int id)
        {
            return offerService.getById(id);
        }

        // POST api/<controller>
        public OfferEntity Post(OfferPostDTO offer)
        {
            return offerService.save(offer);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            offerService.delete(id);
        }
    }
}