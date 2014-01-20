using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ScrapWeb.DataAccess;
using ScrapWeb.Entities;
using ScrapWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ScrapWeb
{
    public class DataInitializer : DropCreateDatabaseAlways<ScrapContext>
    {
        protected override void Seed(ScrapContext context)
        {
            var UserManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!RoleManager.RoleExists("ROLE_ADMIN"))
            {
                RoleManager.Create(new IdentityRole("ROLE_ADMIN"));
            }

            var user = new IdentityUser("admin");
            var admin = UserManager.Create(user, "123456");
            if (admin.Succeeded)
            {
                UserManager.AddToRole(user.Id, "ROLE_ADMIN");
            }

            //TEST (REMOVE)
            var orderRepository = new GenericRepository<OrderEntity>(context);
            var orderLineRepository = new GenericRepository<OrderLineEntity>(context);
            var orderline = new OrderLineEntity
            {
                quantity = 2,
                sgId = "2",
                description = "Ola ke ase"
            };

            var order = new OrderEntity
            {
                sgId = "1",
                deadline = DateTime.Now
            };
            order.lines.Add(orderline);
            orderRepository.Insert(order);
            //orderLineRepository.Insert(orderline);
            
            base.Seed(context);
        }
    }
}