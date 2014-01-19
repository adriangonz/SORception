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
            var sol = c_bd.SolicitudSet.FirstOrDefault(x => x.Id == id);
            if(sol != null)
            { 
                var ofertas = (svcTaller.getOfertas(sol.sg_id)).ToList();
                if (ofertas.Count < 1) return Request.CreateResponse(HttpStatusCode.NoContent, "No existen ofertas");

                return ofertas;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "La solicitud con id " + id + " no existe");
            }
        }

    }
}
