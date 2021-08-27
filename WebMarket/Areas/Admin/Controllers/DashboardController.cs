using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMarket.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Admin/Dashbord
        public ActionResult Index()
        {
            return View();
        }
    }
}