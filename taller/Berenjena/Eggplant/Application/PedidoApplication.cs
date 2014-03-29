using Eggplant.DTO;
using Eggplant.Entity;
using Eggplant.Exceptions;
using Eggplant.ServiceTaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant.Application
{
    public class PedidoApplication : AbstractApplication
    {
        public object getAll()
        {
            return dataService.Pedidos.Get();
        }

        public object getById(int id)
        {
            return dataService.Pedidos.GetFirstWithAllAndDescription(x => x.id == id);
        }

        public object request(PedidoDTO pedidoDTO)
        {
            Pedido p = new Pedido();
            p.solicitud = dataService.Solicitudes.GetByID(pedidoDTO.solicitud);

            // Consigo ya todas las ofertas para esta solicitud para sacar despues los datos y solo hacer una peticion
            var ofertas = sgService.getOfertas(p.solicitud.sg_id);

            foreach (LineaPedidoDTO lpDTO in pedidoDTO.lineas)
            {
                LineaPedido lp = new LineaPedido();
                lp.sg_linea_oferta_id = lpDTO.id_linea_oferta;
                lp.sg_lina_solicitud_id = lpDTO.id_linea_solcitud;
                lp.quantity = lpDTO.cantidad;


                lp.status = LineaPedido.FAILED;

                ExpOfertaLine lineaALaQueResponde = getLineaOfertaById(ofertas, lp.sg_linea_oferta_id);
                lp.price = lineaALaQueResponde.price;
                if (lp.quantity > lineaALaQueResponde.quantity)
                    throw new ApplicationLayerException(System.Net.HttpStatusCode.BadRequest, "No puedes pedir mayor cantidad que la que se ofrece");

                p.rawLines.Add(lp);
            }
            dataService.Pedidos.Insert(p);
            dataService.SaveChanges();
            addPedidoToSG(p);
            return p.id;
        }

        private ExpOfertaLine getLineaOfertaById(List<ExpOferta> ofertas, int sg_id_linea_oferta)
        {
            foreach (var oferta in ofertas)
            {
                var linea = oferta.lineas.FirstOrDefault(x => x.id == sg_id_linea_oferta);
                if (linea != null) return linea;
            }
            throw new ApplicationLayerException(System.Net.HttpStatusCode.NotFound, "La linea " + sg_id_linea_oferta + " no existe en el sg para esa solicitud");
        }

        /// Recorremos el pedido y lo anyadimos el SG
        private void addPedidoToSG(Pedido pedido)
        {

            ExpPedido tr = new ExpPedido();
            List<ExpPedidoLine> lineasHelper = new List<ExpPedidoLine>();
            foreach (var lineaPedido in pedido.rawLines)
            {
                ExpPedidoLine sl = new ExpPedidoLine();
                sl.linea_oferta_id = lineaPedido.sg_linea_oferta_id;
                sl.quantity = lineaPedido.quantity;

                // Anyado la linea al pedido
                lineasHelper.Add(sl);
                lineaPedido.status = LineaPedido.SENT;
            }
            tr.lineas = lineasHelper.ToArray();
            sgService.selectOferta(tr);
            dataService.SaveChanges();
        }
    }
}