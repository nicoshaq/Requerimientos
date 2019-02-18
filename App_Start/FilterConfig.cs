using Requerimientos.App_Start;
using System.Web;
using System.Web.Mvc;

namespace Inspinia_MVC5
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Log4NetExceptionFilter());
        }

        //public static class FilterConfig
        //{
        //    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        //    {
        //        filters.Add(new Log4NetExceptionFilter());
        //    }
        //}
    }
}
