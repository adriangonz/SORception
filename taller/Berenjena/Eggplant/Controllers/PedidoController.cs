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

        public static string FAILED = "FAILED";
        public static string DELETED = "DELETED";
        public static string REQUESTED = "REQUESTED";

        // GET api/pedido
        public object Get()
        {
            var pedidos = c_bd.PedidoSet.AsQueryable().Where(x => x.status != FAILED);
            return pedidos;
        }

        [Route("api/pedido/update")]
        public object GetUpdated()
        {
            UpdatePedidosFromSG();
            return Get();
        }

        // GET api/pedido/5
        public object Get(int id)
        {
            var pedido = c_bd.PedidoSet.AsQueryable().FirstOrDefault(x => x.Id == id);
            return pedido;
        }

        // POST api/pedido
        public object Post([FromBody]JObject values)
        {
            Pedido p = new Pedido();
            p.oferta_id = int.Parse(values["oferta"].ToString());
            p.status = "FAILED";
            p.timeStamp = DateTime.Now;
            ExposedOferta ofer = svcTaller.getOferta(p.oferta_id);
            if (ofer == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "La oferta " + p.oferta_id + " no existe");
            p.Solicitud = c_bd.SolicitudSet.FirstOrDefault(x => x.sg_id == ofer.solicitud_id);
            if (p.Solicitud == null)
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "La solicitud " + ofer.solicitud_id + " asociada a la oferta no existe en la DB interna");
            foreach (JObject item in values["lineas"])
            {
                LineaPedido lp = new LineaPedido();
                lp.linea_oferta_id = int.Parse(item["id_linea_oferta"].ToString());
                ExposedLineaOferta lofer = ofer.lineas.FirstOrDefault(x => x.id_linea == lp.linea_oferta_id);
                if (lofer == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "La linea " + lp.linea_oferta_id + " no existe");
                lp.price = (decimal)lofer.price;
                lp.quantity = int.Parse(item["cantidad"].ToString());
                if (lp.quantity > lofer.quantity)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No puedes pedir mas piezas de las ofertadas");
            }
            c_bd.PedidoSet.Add(p);
            c_bd.SaveChanges();

            addPedidoToSG(p.Id);
            UpdatePedidosFromSG();

            return p;
        }

        // PUT api/pedido/5
        public object Put(int id, [FromBody]JObject values)
        {
            // Estoy pensando que podemos liarnos menos si solo hacemos que se pueda cancelar y volver a crear LO VEO LO VEO
            UpdatePedidosFromSG();
            var pedido = c_bd.PedidoSet.FirstOrDefault(x => x.Id == id);
            if (pedido != null)
            {
                //Los pedidos solo se pueden actualizar si todavia estan pendientes
                if (pedido.status == REQUESTED)
                {
                    ExposedOferta ofer = svcTaller.getOferta(pedido.oferta_id);
                    if (ofer == null)
                        return Request.CreateResponse(HttpStatusCode.NotFound, "La oferta " + pedido.oferta_id + " no existe");
                    foreach (JObject item in values["lineas"])
                    {
                        //TODO ACTUALIZAR EN EL SG

                    }
                    return pedido;
                }
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Los pedidos solo se pueden actualizar si todavia estan pendientes y el pedido esta "+pedido.status);
                
            }
            else return Request.CreateResponse(HttpStatusCode.NotFound, "No existe el pedido "+id);
        }

        // DELETE api/pedido/5
        public object Delete(int id)
        {
            var pedido = c_bd.PedidoSet.AsQueryable().FirstOrDefault(x => x.Id == id);
            if (pedido != null)
            {
                if (pedido.status == REQUESTED)
                {
                    pedido.status = DELETED;
                    //TODO delete from SG
                    c_bd.SaveChanges();

                    return pedido;
                }
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Los pedidos solo se pueden borrar si todavia estan pendientes y el pedido esta " + pedido.status);
               
            }
            else return Request.CreateResponse(HttpStatusCode.NotFound, "No existe el pedido " + id);
        }

        //Recorremos el pedido y lo anyadimos el SG
        private void addPedidoToSG(int idPedido)
        {
            var pedido = c_bd.PedidoSet.FirstOrDefault(x => x.Id == idPedido);
            if (pedido != null)
            {
                TallerResponse tr = new TallerResponse();
                tr.oferta_id = pedido.oferta_id;
                foreach (var lineaPedido in pedido.LineaPedido)
                {
                    TallerResponse.SelectedLine sl = new TallerResponse.SelectedLine();
                    sl.line_id = lineaPedido.linea_oferta_id;//espero que con line_id se refiera a la linea de oferta a la que responde
                    sl.quantity = lineaPedido.quantity;
                    svcTaller.selectOferta(tr);
                }
            }
        }

        //Recorremos todos los pedidos internos y los actualizamos con los datos del SG
        private void UpdatePedidosFromSG()
        {
            var pedidos = c_bd.PedidoSet.AsQueryable().ToList();
            foreach (var pedido in pedidos)
            {
                foreach (var lineaPedido in pedido.LineaPedido)
                {
                    int sgId = lineaPedido.sg_id;
                    //TODO var ofertaSelec = svcTaller.getOfertaSeleccionada(sgId);
                    //Actualizamos todo los campos
                    //lineaPedido.status = ofertaSelec.status; o algo asi
                }
            }
        }
    }
}
