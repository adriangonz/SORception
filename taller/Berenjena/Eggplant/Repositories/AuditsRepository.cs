using Eggplant.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Eggplant.Repositories
{
    public class AuditsRepository : GenericRepository<Audit>
    {
        public AuditsRepository(DbContext context)
            : base(context)
        {

        }
        public void create(string type, string description)
        {
            Audit ad = new Audit();
            ad.type = type;
            ad.descriptcion = description;
            this.Insert(ad);
        }
    }
}