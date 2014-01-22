using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagerSystem.DataAccess;
using ManagerSystem.Entities;
using System.Data.Entity;


namespace ManagerSystem.Repositories
{
    public class GarageRepository : GenericRepository<GarageEntity>
    {
        public GarageRepository(MSContext context) : base(context) { }

        public override void Delete(GarageEntity entityToDelete)
        {
            foreach (var token in entityToDelete.tokens)
            {
                token.deleted = true;
                context.Entry(token).State = EntityState.Modified;
            }
   
            base.Delete(entityToDelete);
        }
    }
}