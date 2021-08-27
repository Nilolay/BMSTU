using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebMarket
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //CategoryMenuPartial
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute("CategoryMenuPartial", "Pages/CategoryMenuPartial", new { controller = "Shop", action = "CategoryMenuPartial" }, new[] { "WebMarket.Controllers" });

            routes.MapRoute("Account", "Account/{action}/{id}", new { controller = "Account", action = "Index", id = UrlParameter.Optional }, new[] { "WebMarket.Controllers" });
            routes.MapRoute("Cart", "Cart/{action}/{id}", new { controller = "Cart", action = "Index", id = UrlParameter.Optional }, new[] { "WebMarket.Controllers" });
            routes.MapRoute("Shop", "Shop/{action}/{name}", new { controller = "Shop", action = "Index", name = UrlParameter.Optional }, new[] { "WebMarket.Controllers" });
            routes.MapRoute("SidebarPartial", "Pages/SidebarPartial", new { controller = "Pages", action = "SidebarPartial" }, new[] { "WebMarket.Controllers" });
            routes.MapRoute("PagesMenuPartial", "Pages/PagesMenuPartial", new { controller = "Pages", action = "PagesMenuPartial" }, new[] { "WebMarket.Controllers" });
            routes.MapRoute("Pages", "{page}", new { controller = "Pages", action = "Index" }, new[] { "WebMarket.Controllers" } );
            routes.MapRoute("Default", "", new { controller = "Pages", action = "Index" }, new[] { "WebMarket.Controllers" });

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
