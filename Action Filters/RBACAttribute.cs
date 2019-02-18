using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


public class RBACAttribute : AuthorizeAttribute
{
    ILog log = LogManager.GetLogger(typeof(RBACAttribute));
    public override void OnAuthorization(AuthorizationContext filterContext)
    {
        //Permisos basados en el nombre del controlador solicitado y el nombre de la acción en el formato 'nombre-controlador-acción'
        string requiredPermission = String.Format("{0}-{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName);

        //Cree una instancia de nuestro objeto de autorización de usuario personalizado que pasa solicitando al usuario 'Nombre de usuario de Windows' en el constructor
        SUIUsuarios requestingUser = new SUIUsuarios(filterContext.RequestContext.HttpContext.User.Identity.Name);
        //Check si el usuario tiene los permisos sobre los controladores o tiene el perfil admin
        if (!requestingUser.HasPermission(requiredPermission) & !requestingUser.EsAdmin)
        {
          
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Index" }, { "controller", "Unauthorised" } });
        }
     
    }
}
