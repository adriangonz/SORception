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
    [Authorize]
    public class AcceptedController : ApiController
    {
        private OfferService offerService;

        AcceptedController() 
        {
            offerService = new OfferService();
        }

        // GET api/<controller>
        public IEnumerable<OfferEntity> Get()
        {
            return offerService.getAccepted();
        }
    }
}