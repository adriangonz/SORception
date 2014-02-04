using Eggplant.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Eggplant.Repositories
{
    public class PedidoRepository : GenericRepository<Pedido>
    {
        public PedidoRepository(DbContext context)
            : base(context)
        {

        }
        public Pedido GetFirstWithAll(
            Expression<Func<Pedido, bool>> filter = null)
        {
            return this.Get(filter, null, "rawLines").First();
        }

        public Pedido GetFirstWithAllAndDescription(
            Expression<Func<Pedido, bool>> filter = null)
        {
            var pedido =  this.Get(filter, null, "rawLines").First();
            foreach (var line in pedido.lines)
            {
                var lineaSolicitud = context.Set<LineaSolicitud>().FirstOrDefault(x => x.sg_id == line.sg_lina_solicitud_id);
                if (lineaSolicitud == null)
                {
                    line.description = "Descripcion";
                }
                else
                {
                    line.description = lineaSolicitud.descripcion;
                }
            }
            return pedido;
        }
    }
}