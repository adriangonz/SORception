using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ScrapWeb.DataAccess;
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

            base.Seed(context);
        }
    }
}