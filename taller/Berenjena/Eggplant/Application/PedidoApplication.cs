using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant.Application
{
    public class PedidoApplication : AbstractApplication
    {
        public object getAll(string userId)
        {
            return dataService.Pedidos.Get(x => x.solicitud.user_id == userId);
        }

        public object getById(int id, string userId)
        {
            return dataService.Pedidos.GetFirstWithAll(x => x.solicitud.user_id == userId && x.id == id);
            /*
            BDBerenjenaContainer c_bd = new BDBerenjenaContainer();
            var userId = User.Identity.GetUserId();
            var pedido = c_bd.PedidoSet.AsQueryable().FirstOrDefault(x => x.Id == id && x.Solicitud.user_id == userId);
            if (pedido == null) return Request.CreateResponse(HttpStatusCode.NotFound, "El pedido " + id + " no existe o no responde a una solicitud del usuario");
            foreach (var linea in pedido.LineaPedido)
            {
                var lineaSolicitud = c_bd.LineaSolicitudSet.FirstOrDefault(x => x.sg_id == linea.sg_id);
                if (lineaSolicitud == null) { 
                    linea.description = "Descripcion";
                }
                else
                {
                    linea.description = lineaSolicitud.descripcion;
                }
            }
            return pedido; */
        }
    }
}