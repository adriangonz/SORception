using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Eggplant.ServiceTaller;

namespace Eggplant.Controllers
{
    public class OfertaController : ApiController
    {
        static BDBerenjenaContainer c_bd = new BDBerenjenaContainer();
        static Eggplant.ServiceTaller.GestionTallerClient svcTaller = new Eggplant.ServiceTaller.GestionTallerClient();




        // GET api/oferta/5
        public object Get(int id)
        {
            var solicitudes = (svcTaller.getOfertas(id));
            if (solicitudes.Length < 1) return Request.CreateResponse(HttpStatusCode.NotFound, "No existen ofertas");
            return solicitudes;
        }
    }
}
