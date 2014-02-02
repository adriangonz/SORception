using Eggplant.DataAcces;
using Eggplant.Entity;
using Eggplant.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eggplant.Services
{
    public class DataService : IDataService
    {
        private EggplantContext context;
        private TokenRepository tokens;

        public DataService()
        {
            context = new EggplantContext();
        }
        public TokenRepository Tokens
        {
            get
            {
                if (tokens == null)
                    tokens = new TokenRepository(context);
                return tokens;
            }
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        
    }
}