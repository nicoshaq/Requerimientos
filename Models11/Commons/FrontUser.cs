using SUI_Reportes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SUI_Reportes.Models.Commons
{
    public class FrontUser
    {
        public static bool TienePermiso(RolesPermisos valor)
        {
            var usuario = Get();
            return !usuario.Rol.Permiso.Where(x => x.PermisoID == valor)
                .Any();
        }

        public static Usuario Get()
        {
            return new Usuario().Obtener(SessionHelper.GetUser());
        }
    }
}