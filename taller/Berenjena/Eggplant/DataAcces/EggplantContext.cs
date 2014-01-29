using Eggplant.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Eggplant.DataAcces
{
    public class EggplantContext : IdentityDbContext<IdentityUser>
    {
        public EggplantContext()
            : base("DefaultConnection")
        {

        }
    
        public DbSet<Token> TokensSet { get; set; }
        public DbSet<Solicitud> SolicitudSet { get; set; }
        public DbSet<LineaSolicitud> LineaSolicitudSet { get; set; }
        public DbSet<Pedido> PedidoSet { get; set; }
        public DbSet<LineaPedido> LineaPedidoSet { get; set; }

        // Override for created and modified dates
        public override int SaveChanges()
        {
            var trackables = ChangeTracker.Entries<AbstractEntity>();

            if (trackables != null)
            {
                foreach (var item in trackables.Where(t => t.State == EntityState.Added))
                {
                    item.Entity.creationDate = System.DateTime.Now;
                    item.Entity.updatedDate = System.DateTime.Now;
                }

                foreach (var item in trackables.Where(t => t.State == EntityState.Modified))
                {
                    item.Entity.updatedDate = System.DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}