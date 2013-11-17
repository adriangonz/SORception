using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AdminManager.Controllers
{
    public class TallerController : ApiController
    {
        // PUT api/taller/5
        public void Put(int id, [FromBody]JObject data)
        {
            ServiceAdmin.GestionAdminClient gd = new ServiceAdmin.GestionAdminClient();
            gd.activeTaller(id, bool.Parse(data["active"].ToString()));
        }

        // DELETE api/taller/5
        public void Delete(int id)
        {
        }
    }
}
