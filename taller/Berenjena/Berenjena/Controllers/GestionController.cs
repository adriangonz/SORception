using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Berenjena.Controllers
{
    public class GestionController : ApiController
    {
        // TODO: HAY QUE SACAR EL ID DE LAS COOKIES

        ServiceTaller.GestionTallerClient svcTaller = new ServiceTaller.GestionTallerClient();
        // GET api/gestion
        public object Get()
        {
            ServiceTaller.Taller t = svcTaller.getTaller(1);
            return (new { id= t.Id, name= t.name, active=t.active });
        }

        // POST api/gestion
        public void Post([FromBody]JObject value)
        {
            svcTaller.addTaller(value["nombre"].ToString());
        }

        // PUT api/gestion
        public void Put([FromBody]JObject value)
        {
            ServiceTaller.Taller t = svcTaller.getTaller(1);
            t.name = value["nombre"].ToString();
            svcTaller.putTaller(t);
        }

        // DELETE api/gestion
        public void Delete()
        {
            svcTaller.deleteTaller(1);
        }

        // GET api/gestion/token
        [Route("api/gestion/token")]
        public string GetToken() { 
            return "asdfasdfTomaTokenInventao";
        }
    }
}
