using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;


public static class RBAC_ExtendedMethods
{
    public static bool HasRole(this ControllerBase controller, string role)
    {
        bool bFound = false;
        try
        {
            //Se fija si el usuario tiene el rol especificado
            bFound = new SUIUsuarios(controller.ControllerContext.HttpContext.User.Identity.Name).HasRole(role);            
        }
        catch { }
        return bFound;
    }

    public static bool HasRoles(this ControllerBase controller, string roles)
    {
        bool bFound = false;
        try
        {
            // Revisa si el usuario tiene cualquiera de los roles
           
            bFound = new SUIUsuarios(controller.ControllerContext.HttpContext.User.Identity.Name).HasRoles(roles);
        }
        catch { }
        return bFound;
    }

    public static bool HasPermission(this ControllerBase controller, string permission)
    {
        bool bFound = false;
        try
        {
            //Se fija si el usuario tiene los permisos necesarios
            bFound = new SUIUsuarios(controller.ControllerContext.HttpContext.User.Identity.Name).HasPermission(permission);
        }
        catch { }
        return bFound;
    }

    public static bool IsSysAdmin(this ControllerBase controller)
    {        
        bool bIsSysAdmin = false;
        try
        {
            //Se fija si el usuario tiene los privilegios de Administrador
            bIsSysAdmin = new SUIUsuarios(controller.ControllerContext.HttpContext.User.Identity.Name).EsAdmin;
        }
        catch { }
        return bIsSysAdmin;
    }
}
