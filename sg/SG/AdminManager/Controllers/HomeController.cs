using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminManager.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            ServiceReference1.GestionDesguaceClient gd = new ServiceReference1.GestionDesguaceClient();
            var desguaces = gd.getAll();
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View(desguaces.ToList());
        }
    }
}
