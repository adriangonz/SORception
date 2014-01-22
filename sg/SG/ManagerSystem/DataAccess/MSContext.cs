using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ManagerSystem.Entities;

namespace ManagerSystem.DataAccess
{
    public class MSContext : DbContext
    {
        DbSet<TokenEntity> Token { get; set; }
        DbSet<GarageEntity> Garage { get; set; }
        DbSet<OrderEntity> Order { get; set; }
        DbSet<OrderLineEntity> OrderLine { get; set; }
        DbSet<JunkyardEntity> Junkyard { get; set; }
        DbSet<OfferEntity> Offer { get; set; }
        DbSet<OfferLineEntity> OfferLine { get; set; }

        // Override for created and modified dates
        public override int SaveChanges()
        {
            var trackables = ChangeTracker.Entries<BaseEntity>();

            if (trackables != null)
            {
                foreach (var item in trackables.Where(t => t.State == EntityState.Added))
                {
                    item.Entity.created_at = System.DateTime.Now;
                    item.Entity.updated_at = System.DateTime.Now;
                }

                foreach (var item in trackables.Where(t => t.State == EntityState.Modified))
                {
                    item.Entity.updated_at = System.DateTime.Now;
                }

                // As per GenericRepository.cs:77 this will never get called
                foreach (var item in trackables.Where(t => t.State == EntityState.Deleted))
                {
                    item.Entity.updated_at = System.DateTime.Now;
                    item.Entity.deleted = true;
                    item.State = EntityState.Modified;
                }
            }

            return base.SaveChanges();
        }
    }
}