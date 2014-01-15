using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Eggplant.ServiceTaller;
using Newtonsoft.Json.Linq;

namespace Eggplant.Controllers
{
    public class PedidoController : ApiController
    {
        static BDBerenjenaContainer c_bd = new BDBerenjenaContainer();
        static Eggplant.ServiceTaller.GestionTallerClient svcTaller = new Eggplant.ServiceTaller.GestionTallerClient();

        // GET api/pedido
        public object Get()
        {
            TallerResponse tr = new TallerResponse();
            TallerResponse.SelectedLine sl = new TallerResponse.SelectedLine();
            
            sl.quantity = 5;//tr.selected_lines
            return new string[] { "value1", "value2" };
        }

        // GET api/pedido/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/pedido
        public object Post([FromBody]JObject values)
        {
            Pedido p = new Pedido();
            p.oferta_id = int.Parse(values["oferta"].ToString());
            p.status = "FAILED";
            p.timeStamp = DateTime.Now;
            ExposedOferta ofer = new ExposedOferta();//TODO SG getOferta
            if (ofer == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "La oferta " + p.oferta_id + " no existe");
            int idSolicitud = ofer.solicitud_id;
            p.Solicitud = c_bd.SolicitudSet.FirstOrDefault(x => x.Id == idSolicitud);
            foreach (JObject item in values["lineas"])
            {
                LineaPedido lp = new LineaPedido();
                lp.linea_oferta_id = int.Parse(item["id_linea_oferta"].ToString());
                ExposedLineaOferta lofer = ofer.lineas.FirstOrDefault(x => x.id_linea == lp.linea_oferta_id);
                if (lofer == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "La linea "+lp.linea_oferta_id+" no existe");
                lp.price = (decimal)lofer.price;
                lp.quantity = int.Parse(item["cantidad"].ToString());
                if (lp.quantity > lofer.quantity) 
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No puedes pedir mas piezas de las ofertadas");
            }
            c_bd.PedidoSet.Add(p);
            c_bd.SaveChanges();
            return p;
        }

        // PUT api/pedido/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/pedido/5
        public void Delete(int id)
        {
        }
    }
}
