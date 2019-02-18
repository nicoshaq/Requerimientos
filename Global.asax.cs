using Requerimientos;
using Requerimientos.Models;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Inspinia_MVC5
{

    public class MvcApplication : HttpApplication
    {
        //private string db = ConfigurationManager.ConnectionStrings["RequerimientosConn"].ConnectionString;

       // static readonly string con = @"data source=DESKTOP-1REMSUJ\SQLEXPRESS;initial catalog=prueba;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework;";

        //string con = ConfigurationManager.ConnectionStrings["RequerimientosConn"].ConnectionString;
        protected void Application_Start()
        {
           
            log4net.Config.XmlConfigurator.Configure();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);




           // SqlDependency.Start(@"data source=DESKTOP-BN17KL9\SQLEXPRESS;initial catalog=requerimientos;integrated security=True;");
        }


       
      

        //protected void Session_Start(object sender, EventArgs e)
        //{
        //    NotificationComponent NC = new NotificationComponent();
        //    var currentTime = DateTime.Now;
        //    HttpContext.Current.Session["LastUpdated"] = currentTime;
        //    NC.RegisterNotification(currentTime);
        //}
        //protected void Application_End()
        //{
            
        //    SqlDependency.Stop(con);
        //}
    }
}
