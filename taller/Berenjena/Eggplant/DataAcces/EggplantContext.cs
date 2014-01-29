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
    }
}