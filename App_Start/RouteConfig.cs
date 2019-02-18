using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Inspinia_MVC5
{
    public class RouteConfig
    {
        string userName = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name;

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                 defaults: new { controller = "Mensajes", action = "Index", id = UrlParameter.Optional },
           //defaults: new { controller = "Dashboards", action = "Dashboard_1", id = UrlParameter.Optional },
           namespaces: new string[] { "Requerimientos.Controllers" });
        }

    }
}
