using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Eggplant.ServiceTaller;
using Eggplant.Application;

namespace Eggplant.Controllers
{
    [Authorize(Roles = "ROLE_ADMIN")]
    public class SettingsController : ApiController
    {
        internal SettingsApplication sa;

        public SettingsController()
        {
            sa = new SettingsApplication();
        }

        // GET api/settings
        public object Get()
        {
            IEnumerable<Entity.Token> tokens = sa.getAll();
            return (new { name = "Taller RESCEPT NIGGA", tokens });
        }

        // POST api/settings
        public object Post(JObject requestJSON)
        {
            return sa.Request(requestJSON["nombre"].ToString());
        }
        
    }
}
