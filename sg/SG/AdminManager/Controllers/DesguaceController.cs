using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.EntityClient;

namespace AdminManager.Controllers
{
    public class DesguaceController : ApiController
    {
        // GET api/desguace
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/desguace/5
        public string Get(int id)
        {
            /*using (managersystemEntities ms_ent = new managersystemEntities())
            {
                var desguace = (from d in ms_ent.DesguaceConjunto
                                where d.id == id
                                select d).First();
                desguace.active = true;
                ms_ent.SaveChanges();
            }*/
            return "value";
        }

        // POST api/desguace
        public void Post([FromBody]string value)
        {
        }

        // PUT api/desguace/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/desguace/5
        public void Delete(int id)
        {
        }
    }
}
