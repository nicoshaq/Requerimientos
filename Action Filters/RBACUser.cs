using RBACModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Requerimientos.Models;
using log4net;

public class SUIUsuarios
{
    ILog log = LogManager.GetLogger(typeof(RBACAttribute));
    public int User_Id { get; set; }
    public bool EsAdmin { get; set; }
    public string Usuario { get; set; }
    private List<UsuarioRol> Roles = new List<UsuarioRol>();

    public SUIUsuarios(string _usuario)
    {
        this.Usuario = _usuario;
        this.EsAdmin = false;
        GetDatabaseUserRolesPermisos();
    }

    private void GetDatabaseUserRolesPermisos()
    {
        using (RequerimientosConn _data = new RequerimientosConn())
        {            
            Usuarios _user = _data.Usuarios.Where(u => u.Usuario == this.Usuario).FirstOrDefault();
            if (_user != null)
            {
                this.User_Id = _user.User_Id;
                foreach (ROLES _rol in _user.ROLES)
                {
                    UsuarioRol _userRole = new UsuarioRol { Rol_Id = _rol.Rol_Id, Nombre = _rol.Nombre };
                    foreach (Permisos _permissions in _rol.Permisos)
                    {
                        _userRole.Permisos.Add(new RolPermiso { Permiso_Id = _permissions.Permiso_Id, Descripcion = _permissions.Descripcion });
                    }
                    this.Roles.Add(_userRole);

                    if (!this.EsAdmin)
                        this.EsAdmin = _rol.EsAdmin;
                }
            }
        }
    }

    public bool HasPermission(string requiredPermission)
    {
        bool bFound = false;
        foreach (UsuarioRol role in this.Roles)
        {
            try
            {
                bFound = (role.Permisos.Where(p => p.Descripcion.ToLower() == requiredPermission.ToLower()).ToList().Count > 0);
                if (bFound)
                    break;
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        return bFound;
    }

    public bool HasRole(string role)
    {
        return (Roles.Where(p => p.Nombre == role).ToList().Count > 0);
    }

    public bool HasRoles(string roles)
    {
        bool bFound = false;
        string[] _roles = roles.ToLower().Split(';');
        foreach (UsuarioRol role in this.Roles)
        {
            try
            {
                bFound = _roles.Contains(role.Nombre.ToLower());
                if (bFound)
                    return bFound;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        return bFound;
    }
}

public class UsuarioRol
{
    public int Rol_Id { get; set; }
    public string Nombre { get; set; }
    public List<RolPermiso> Permisos = new List<RolPermiso>();
}

public class RolPermiso
{
    public int Permiso_Id { get; set; }
    public string Descripcion { get; set; }
}