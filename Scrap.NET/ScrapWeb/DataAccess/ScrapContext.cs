using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ScrapWeb.DataAccess
{
    public class ScrapContext : 
        IdentityDbContext<IdentityUser>
    {

    }
}