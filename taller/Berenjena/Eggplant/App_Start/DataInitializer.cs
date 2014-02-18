using Eggplant.DataAcces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Eggplant
{
    public class DataInitializer : DropCreateDatabaseIfModelChanges<EggplantContext>
    {
        protected override void Seed(EggplantContext context)
        {
            var UserManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));

            var user = new IdentityUser("admin");
            var admin = UserManager.Create(user, "123456");

            base.Seed(context);
        }
    }
}