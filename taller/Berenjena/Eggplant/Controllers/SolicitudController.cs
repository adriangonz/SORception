using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Eggplant.Controllers
{
    [Authorize]
    [RoutePrefix("api/solicitud")]
    public class SolicitudController : ApiController
    {
        // GET api/solicitud
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/solicitud/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/solicitud
        public void Post([FromBody]string value)
        {
        }

        // PUT api/solicitud/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/solicitud/5
        public void Delete(int id)
        {
        }
    }
}
