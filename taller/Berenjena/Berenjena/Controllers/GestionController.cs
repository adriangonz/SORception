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
        // GET api/gestion
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/gestion
        public void Post([FromBody]string value)
        {
        }

        // PUT api/gestion/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/gestion/5
        public void Delete(int id)
        {
        }
    }
}
