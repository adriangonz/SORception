using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Eggplant.ServiceTaller;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Eggplant.Models;
using System.Web;
using Microsoft.AspNet.Identity;
using Eggplant.Application;
using Eggplant.DTO;

namespace Eggplant.Controllers
{
    [Authorize]
    [RoutePrefix("api/solicitud")]
    public class SolicitudController : ApiController
    {
        private SolicitudApplication sa;
        public SolicitudController()
        {
            sa = new SolicitudApplication();
        }


        // GET api/solicitud
        public object Get()
        {
            return sa.getByUser();
        }

        // GET api/solicitud/5
        public object Get(int id)
        {
            return sa.getDetailById(id);
        }

        // POST api/solicitud
        public object Post(SolicitudPostDTO data)
        {
            return sa.Request(data);
        }

        // PUT api/solicitud/5
        public object Put(int id, SolicitudPostDTO data)
        {
            
            return sa.PutRequest(id, data);
        }

        // DELETE api/solicitud/5
        public void Delete(int id)
        {
            sa.deleteSolicitud(id);
        }
    }
}
