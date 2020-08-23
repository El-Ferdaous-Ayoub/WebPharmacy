using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Config.CheckData();
            if (Config.DataExist == 2)
                return RedirectToRoute(new { controller = "Pharmacy", action = "Create", t = 1 });
            else if (Config.DataExist == 1)
                return RedirectToRoute(new { controller = "Employees", action = "Create", t = 1 });
            else if (Account.User == null || Account.Department == null)
                return RedirectToRoute(new { controller = "Account", action = "Login" });
            return View();
        }

        public ActionResult Contact()
        {
            Config.CheckData();
            if (Config.DataExist == 2)
                return RedirectToRoute(new { controller = "Pharmacy", action = "Create", t = 1 });
            else if (Config.DataExist == 1)
                return RedirectToRoute(new { controller = "Employees", action = "Create", t = 1 });
            else if (Account.User == null || Account.Department == null)
                return RedirectToRoute(new { controller = "Account", action = "Login" });
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}