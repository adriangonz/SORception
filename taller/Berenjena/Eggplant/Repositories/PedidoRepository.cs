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
    }
}