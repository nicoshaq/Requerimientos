using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SUI_Reportes.Models.Commons;
using System.Web.Mvc;
using System.Web.Routing;

namespace SUI_Reportes.Models.Commons
{
    public class PermisoAttribute : ActionFilterAttribute
    {
        public RolesPermisos Permiso { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!FrontUser.TienePermiso(Permiso))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "Index"
                }));
            }
        }
    }
}