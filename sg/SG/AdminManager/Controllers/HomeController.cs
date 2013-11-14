using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminManager.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ServiceAdmin.GestionAdminClient gd = new ServiceAdmin.GestionAdminClient();
            var desguaces = gd.getDesguaces();
            ViewBag.Message = "Tenemos "+desguaces.Count()+ " deguaces y  32 talleres";

            return View();
        }

        [Authorize]
        public ActionResult Desguaces()
        {
            ServiceAdmin.GestionAdminClient gd = new ServiceAdmin.GestionAdminClient();
            var desguaces = gd.getDesguaces();
            return View(desguaces.ToList());
        }

        [Authorize]
        public ActionResult Talleres()
        {
            return View();
        }
    }
}
