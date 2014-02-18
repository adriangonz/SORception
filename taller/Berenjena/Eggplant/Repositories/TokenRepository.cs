using Eggplant.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Eggplant.Repositories
{
    public class TokenRepository : GenericRepository<Token>
    {
        public TokenRepository(DbContext context):base(context) {  }

        public void updateStatus(string newStatus)
        {
            var tokens = dbSet.ToList();
            foreach (var t in tokens) { t.status = newStatus; }
        }
    }
}