using Eggplant.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Eggplant.Repositories
{
    public class SolicitudRespository : GenericRepository<Solicitud>
    {
        public SolicitudRespository(DbContext context):base(context)
        {

        }

        public IEnumerable<Solicitud> getAllWithAll(
            Expression<Func<Solicitud, bool>> filter = null)
        {
            return this.Get(filter, null, "rawLines");
        }

        public Solicitud GetFirstWithAll(
            Expression<Func<Solicitud, bool>> filter = null)
        {
            return this.Get(filter, null, "rawLines").First();
        }
        public Solicitud GetFirstWithAllById(int id)
        {
            return this.Get(x => x.id == id, null, "rawLines").First();
        }
    }
}