using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.EntityClient;
using Newtonsoft.Json.Linq;

namespace AdminManager.Controllers
{
    public class DesguaceController : ApiController
    {
        // PUT api/desguace/5
        public void Put(int id, [FromBody]JObject data)
        {
            ServiceAdmin.GestionAdminClient gd = new ServiceAdmin.GestionAdminClient();
            gd.activeDesguace(id, bool.Parse(data["active"].ToString()));
        }

        // DELETE api/desguace/5
        public void Delete(int id)
        {
        }
    }
}
