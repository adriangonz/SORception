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
    [RoutePrefix("api/token")]
    public class TokenController : ApiController
    {
        private TokenService tokenService;

        TokenController()
        {
            tokenService = new TokenService();
        }

        // GET api/<controller>
        public TokenEntity Get()
        {
            return tokenService.getValid();
        }

        // POST api/<controller>
        public TokenEntity Post()
        {
            return tokenService.requestToken();
        }

        [Route("list")]
        public IEnumerable<TokenEntity> GetAll()
        {
            return tokenService.getAll();
        }
    }
}