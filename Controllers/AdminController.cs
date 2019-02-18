using RBACModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Requerimientos.Models;
using log4net;
//using Requerimientos.Models;

//using Requerimientos.Models;

namespace Requerimientos.Models
{
    
    // [RBAC]
    public class AdminController : Controller
    {
        ILog log = LogManager.GetLogger(typeof(AdminController));
        string userName = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name;
        private RequerimientosConn database = new RequerimientosConn();
        private int? idearea;
        private int? areas;

        #region USERS
        // GET: Admin


        public ActionResult Index()
        {
           


                return View(database.Usuarios.Where(r => r.Inactivo == false || r.Inactivo == null).OrderBy(r => r.Apellido).ThenBy(r => r.Nombre).ToList());
            
        }


        public ViewResult ListaUsuarios(int id)
        {
            Usuarios user = database.Usuarios.Find(id);
            SetViewBagData(id);
            return View(user);
        }

        public ActionResult CrearUsuario()
        {

            ViewBag.Idarea = new SelectList(database.Area, "Idarea", "Descripcion");
            return View();
        }

        [HttpPost]
        public ActionResult CrearUsuario(Usuarios user)
        {
            if (user.Usuario == "" || user.Usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Username cannot be blank");
                log.Error("Usuario no puede ser nulo");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    List<string> results = database.Database.SqlQuery<String>(string.Format("SELECT Usuario FROM Usuarios WHERE Usuario = '{0}'", user.Usuario)).ToList();
                    bool _userExistsInTable = (results.Count > 0);

                    Usuarios _user = null;
                    if (_userExistsInTable)
                    {
                        _user = database.Usuarios.Where(p => p.Usuario == user.Usuario).FirstOrDefault();
                        if (_user != null)
                        {
                            if (_user.Inactivo == false)
                            {
                                ModelState.AddModelError(string.Empty, "El usuario ya existe!");
                                log.Error("El usuario ya se encuentra registrado");
                                //} else if (Idarea == 0)
                                //{
                                //    TempData["error"] = "jajajajajajajaja";
                            }
                            else
                            {
                                database.Entry(_user).Entity.Inactivo = false;
                                database.Entry(_user).Entity.Modificado = System.DateTime.Now;
                                database.Entry(_user).State = EntityState.Modified;
                                database.SaveChanges();
                                log.InfoFormat("Se ingreso un nuevo usuario {0}", user.Usuario);
                                return RedirectToAction("Index");
                            }
                        }
                    }
                    else
                    {
                        _user = new Usuarios();
                        _user.Usuario = user.Usuario;
                        _user.Apellido = user.Apellido;
                        _user.Nombre = user.Nombre;
                        _user.Titulo = user.Titulo;
                        _user.Inicial = user.Inicial;
                        _user.EMail = user.EMail;
                      //  _user.Idarea = user.Idarea;
                        _user.Inactivo = false;
                      

                        if (ModelState.IsValid)
                        {
                           //_user.Idarea = Idearea;
                            _user.Inactivo = false;
                            _user.Modificado = System.DateTime.Now;

                            database.Usuarios.Add(_user);
                            database.SaveChanges();
                            log.Info(_user.ToString());
                            return RedirectToAction("Index");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
               
            }
            TempData["error"] = "El usuario ya se encuentra registrado";
            ViewBag.Idarea = new SelectList(database.Area, "Idarea", "Descripcion");
           
            return View(user);
        }

        [HttpGet]
        public ActionResult EditarUsuario(int id)
        {

           

            Usuarios user = database.Usuarios.Find(id);
            areas = user.Idarea;
            ViewBag.Idarea = new SelectList(database.Area, "Idarea", "Descripcion", areas);
            SetViewBagData(id);
          

            return View(user);

        }

        [HttpPost]
        public ActionResult EditarUsuario(Usuarios user)
        {
            Usuarios _user = database.Usuarios.Where(p => p.User_Id == user.User_Id).FirstOrDefault();
            if (_user != null)
            {
                try
                {
                    database.Entry(_user).CurrentValues.SetValues(user);
                    database.Entry(_user).Entity.Modificado = System.DateTime.Now;
                    log.InfoFormat("Se edito con exito el usuario {0}", user.Usuario);
                    database.SaveChanges();
                }
                catch (Exception ex)
                {
                    log.Error(ex.ToString());
                   // return ViewBag.result(ex);
                }                
            }
            return RedirectToAction("ListaUsuarios", new RouteValueDictionary(new { id = user.User_Id }));
        }

        [HttpPost]
        public ActionResult ListaUsuarios(Usuarios user)
        {
            if (user.Usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Usuario invalido");

            }

            if (ModelState.IsValid)
            {
                database.Entry(user).Entity.Inactivo = user.Inactivo;
                database.Entry(user).Entity.Modificado = System.DateTime.Now;
                database.Entry(user).State = EntityState.Modified;
                database.SaveChanges();
            }      
            return View(user);
        }

        [HttpGet]
        public ActionResult DeleteUserRole(int id, int userId)
        {
            ROLES role = database.ROLES.Find(id);
            Usuarios user = database.Usuarios.Find(userId);

            if (role.Usuarios.Contains(user))
            {
                role.Usuarios.Remove(user);
                database.SaveChanges();
                log.InfoFormat("Se elimino el usuario {0}", user.Usuario);
            }
            return RedirectToAction("EditarUsuario", "Admin", new { id = userId });
        }

        [HttpGet]
        public PartialViewResult filter4Users(string _surname)
        {
            return PartialView("_ListaUsuarios", GetFilteredUserList(_surname));
        }

        [HttpGet]
        public PartialViewResult filterReset()
        {
            return PartialView("_ListaUsuarios", database.Usuarios.Where(r => r.Inactivo == false || r.Inactivo == null).ToList());
        }

        [HttpGet]
        public PartialViewResult DeleteUserReturnPartialView(int userId)
        {
            try
            {
                Usuarios user = database.Usuarios.Find(userId);
                if (user != null)
                {            
                    database.Entry(user).Entity.Inactivo = true;
                    database.Entry(user).Entity.User_Id = user.User_Id;
                    database.Entry(user).Entity.Modificado = System.DateTime.Now;
                    database.Entry(user).State = EntityState.Modified;

                   
                    database.SaveChanges();
                    return this.filterReset();
                    //return PartialView("_ListaUsuarios", database.Usuarios.Where(r => r.Inactivo == false || r.Inactivo == null).ToList());
                }
            }
            catch (Exception ex)
             
            {
                log.Error(ex.ToString());
            }
           // return PartialView("_ListaUsuarios4Roles");
            return PartialView("_ListaUsuarios", database.Usuarios.Where(r => r.Inactivo == false || r.Inactivo == null).ToList());
           // return this.filterReset();
        }

        private IEnumerable<Usuarios> GetFilteredUserList(string _surname)
        {
            IEnumerable<Usuarios> _ret = null;
            try
            {
                if (string.IsNullOrEmpty(_surname))
                {
                    _ret = database.Usuarios.Where(r => r.Inactivo == false || r.Inactivo == null).ToList();
                }
                else
                {
                    _ret = database.Usuarios.Where(p => p.Apellido == _surname).ToList();
                }
            }
            catch
            {
            }
            return _ret;
        }

        protected override void Dispose(bool disposing)
        {
            database.Dispose();
            base.Dispose(disposing);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteUserRoleReturnPartialView(int id, int userId)
        {
            ROLES role = database.ROLES.Find(id);
            Usuarios user = database.Usuarios.Find(userId);

            if (role.Usuarios.Contains(user))
            {
                role.Usuarios.Remove(user);
                database.SaveChanges();
            }
            SetViewBagData(userId);
            return PartialView("_ListaUsuariosRoles", database.Usuarios.Find(userId));
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddUserRoleReturnPartialView(int id, int userId)//4
        {
            ROLES role = database.ROLES.Find(id);
            Usuarios user = database.Usuarios.Find(userId);

            if (!role.Usuarios.Contains(user))
            {
                role.Usuarios.Add(user);
                database.SaveChanges();
            }
          

            SetViewBagData(userId);
            return PartialView("_ListaUsuariosRoles", database.Usuarios.Find(userId));
        }

        private void SetViewBagData(int _userId)
        {
            ViewBag.UserId = _userId;
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            ViewBag.RoleId = new SelectList(database.ROLES.OrderBy(p => p.Nombre), "Rol_Id", "Nombre");
        }

        public List<SelectListItem> List_boolNullYesNo()
        {
            var _retVal = new List<SelectListItem>();
            try
            {
                _retVal.Add(new SelectListItem { Text = "Not Set", Value = null });
                _retVal.Add(new SelectListItem { Text = "Si", Value = bool.TrueString });
                _retVal.Add(new SelectListItem { Text = "No", Value = bool.FalseString });
            }
            catch { }
            return _retVal;
        }
        #endregion

        #region ROLES
        public ActionResult RolIndex()
        {
            return View(database.ROLES.OrderBy(r => r.Descripcion).ToList());
        }

        public ViewResult ListaRoles(int id)
        {
            Usuarios user = database.Usuarios.Where(r => r.Usuario == User.Identity.Name).FirstOrDefault();
            ROLES role = database.ROLES.Where(r => r.Rol_Id == id)
                   .Include(a => a.Permisos)
                   .Include(a => a.Usuarios)
                   .FirstOrDefault();

            // USERS combo
            ViewBag.UserId = new SelectList(database.Usuarios.Where(r => r.Inactivo == false || r.Inactivo == null), "User_Id", "Usuario");
            ViewBag.RoleId = id;

            // Rights combo
            ViewBag.permisoId = new SelectList(database.Permisos.OrderBy(a => a.Descripcion), "Permiso_Id", "Descripcion");
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();

            return View(role);
        }

        public ActionResult CrearRol()
        {
            Usuarios user = database.Usuarios.Where(r => r.Usuario == User.Identity.Name).FirstOrDefault();
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            return View();
        }

        [HttpPost]
        public ActionResult CrearRol(ROLES _role)
        {
            if (_role.Descripcion == null)
            {
                ModelState.AddModelError("Role Description", "Role Description must be entered");
                log.Warn("Se debe ingresar una descripicion para el rol");
            }

            Usuarios user = database.Usuarios.Where(r => r.Usuario == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {

                _role.Modificado = DateTime.Now;
                database.ROLES.Add(_role);
                database.SaveChanges();
                log.InfoFormat("Se creo correctamente el rol {0}", _role.Nombre);
                return RedirectToAction("RolIndex");
            }
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            return View(_role);
        }


        public ActionResult EditarRol(int id)
        {
            Usuarios user = database.Usuarios.Where(r => r.Usuario == User.Identity.Name).FirstOrDefault();

            ROLES _role = database.ROLES.Where(r => r.Rol_Id == id)
                    .Include(a => a.Permisos)
                    .Include(a => a.Usuarios)
                    .FirstOrDefault();

            // USERS combo
            ViewBag.UserId = new SelectList(database.Usuarios.Where(r => r.Inactivo == false || r.Inactivo == null), "User_Id", "Usuario");
            ViewBag.RoleId = id;

            // Rights combo
            ViewBag.permisoId = new SelectList(database.Permisos.OrderBy(a => a.Permiso_Id), "Permiso_Id", "Descripcion");
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();

            return View(_role);
        }

        [HttpPost]
        public ActionResult EditarRol(ROLES _role)
        {
            if (string.IsNullOrEmpty(_role.Descripcion))
            {
                ModelState.AddModelError("Role Description", "Role Description must be entered");
                log.Warn("Se debe ingresar una descripicion para el rol");
            }

            //EntityState state = database.Entry(_role).State;
            Usuarios user = database.Usuarios.Where(r => r.Usuario == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
              
                database.Entry(_role).State = EntityState.Modified;
                _role.Modificado = DateTime.Now;
                database.SaveChanges();
                log.InfoFormat("Se modifico correctamente el rol {0}", _role.Nombre);
                return RedirectToAction("ListaRoles", new RouteValueDictionary(new { id = _role.Rol_Id }));
            }
            // USERS combo
            ViewBag.UserId = new SelectList(database.Usuarios.Where(r => r.Inactivo == false || r.Inactivo == null), "User_Id", "Usuario");

            // Rights combo
            ViewBag.permisoId = new SelectList(database.Permisos.OrderBy(a => a.Permiso_Id), "Permiso_Id", "Descripcion");
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            return View(_role);
        }


        public ActionResult RoleDelete(int id)
        {
            ROLES _role = database.ROLES.Find(id);
            if (_role != null)
            {
                _role.Usuarios.Clear();
                _role.Permisos.Clear();

                database.Entry(_role).State = EntityState.Deleted;
                database.SaveChanges();
            }
            return RedirectToAction("RolIndex");
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteUserFromRoleReturnPartialView(int id, int userId)
        {
            ROLES role = database.ROLES.Find(id);
            Usuarios user = database.Usuarios.Find(userId);

            if (role.Usuarios.Contains(user))
            {
                role.Usuarios.Remove(user);
                database.SaveChanges();
            }
            return PartialView("_ListaUsuarios4Roles", role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddUser2RoleReturnPartialView(int id, int userId)
        {
            ROLES role = database.ROLES.Find(id);
            Usuarios user = database.Usuarios.Find(userId);

            if (!role.Usuarios.Contains(user))
            {
                role.Usuarios.Add(user);
                database.SaveChanges();
            }
            return PartialView("_ListaUsuarios4Roles", role);
        }

        #endregion












        #region AREAS

        public ViewResult AreasIndex()
        {
            List<Area> _areas = database.Area
                               .OrderBy(wn => wn.Descripcion)
                              // .Include(a => a.ROLES)
                               .ToList();
            return View(_areas);
        }



        public ActionResult CrearArea()
        {
            Usuarios user = database.Usuarios.Where(r => r.Usuario == User.Identity.Name).FirstOrDefault();
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            return View();
        }

        [HttpPost]
        public ActionResult CrearArea(Area _area)
        {
            if (_area.Descripcion == null)
            {
                ModelState.AddModelError("Role Description", "Role Description must be entered");
            }

            Usuarios user = database.Usuarios.Where(r => r.Usuario == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {

               // _area.Modificado = DateTime.Now;
                database.Area.Add(_area);
                database.SaveChanges();
                log.InfoFormat("Se creo correctamente el area {0}", _area.Descripcion);
                return RedirectToAction("AreasIndex");
            }
            ViewBag.List_boolNullYesNo = this.List_boolNullYesNo();
            return View(_area);
        }




        public ActionResult Borrar(int id)
        {
            Area areas = database.Area.Find(id);
            try
            {



                database.Area.Remove(areas);
                database.SaveChanges();
                TempData["success"] = "El area se eliminó correctamente";
                log.InfoFormat("se elimino correctamente el area {0}", areas.Descripcion);
                return RedirectToAction("AreasIndex");

            }
            catch (Exception ex)
            {
                TempData["error"] = "El Area tiene usuario/s asociado/s";
                log.Error(ex);
            }

            return RedirectToAction("AreasIndex");


        }








        #endregion









        #region PERMISSIONS

        public ViewResult PermisosIndex()
        {
            List<Permisos> _permisos = database.Permisos
                               .OrderBy(wn => wn.Descripcion)
                               .Include(a => a.ROLES)
                               .ToList();
            return View(_permisos);
        }

        public ViewResult ListaPermisos(int id)
        {
            Permisos _permiso = database.Permisos.Find(id);
            return View(_permiso);
        }

        public ActionResult CrearPermiso()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CrearPermiso(Permisos _permiso)
        {
            if (_permiso.Descripcion == null)
            {
                ModelState.AddModelError("Permission Description", "Permission Description must be entered");
            }

            if (ModelState.IsValid)
            {
                database.Permisos.Add(_permiso);
                database.SaveChanges();
                return RedirectToAction("PermisosIndex");
            }
            return View(_permiso);
        }

        public ActionResult EditarPermiso(int id)
        {
            Permisos _permiso = database.Permisos.Find(id);
            ViewBag.RoleId = new SelectList(database.ROLES.OrderBy(p => p.Descripcion), "Rol_Id", "Descripcion");
            return View(_permiso);
        }

        [HttpPost]
        public ActionResult EditarPermiso(Permisos _permiso)
        {
            if (ModelState.IsValid)
            {
                database.Entry(_permiso).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("ListaPermisos", new RouteValueDictionary(new { id = _permiso.Permiso_Id }));
            }
            return View(_permiso);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult BorrarPermiso(int id)
        {
            Permisos permission = database.Permisos.Find(id);
            if (permission.ROLES.Count > 0)
                permission.ROLES.Clear();

            database.Entry(permission).State = EntityState.Deleted;
            database.SaveChanges();
            return RedirectToAction("PermisosIndex");
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddPermission2RoleReturnPartialView(int id, int permisoId)
        {
            ROLES role = database.ROLES.Find(id);
            Permisos _permiso = database.Permisos.Find(permisoId);

            if (!role.Permisos.Contains(_permiso))
            {
                role.Permisos.Add(_permiso);
                database.SaveChanges();
            }
            return PartialView("_ListaPermisos", role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddAllPermissions2RoleReturnPartialView(int id)
        {
            ROLES _role = database.ROLES.Where(p => p.Rol_Id == id).FirstOrDefault();
            List<Permisos> _permisos = database.Permisos.ToList();
            foreach (Permisos _permiso in _permisos)
            {
                if (!_role.Permisos.Contains(_permiso))
                {
                    _role.Permisos.Add(_permiso);
                 
                }
            }
            database.SaveChanges();
            return PartialView("_ListaPermisos", _role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeletePermissionFromRoleReturnPartialView(int id, int permisoId)
        {
            ROLES _role = database.ROLES.Find(id);
            Permisos _permiso = database.Permisos.Find(permisoId);

            if (_role.Permisos.Contains(_permiso))
            {
                _role.Permisos.Remove(_permiso);
                database.SaveChanges();
            }
            return PartialView("_ListaPermisos", _role);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult DeleteRoleFromPermissionReturnPartialView(int id, int permisoId)
        {
            ROLES role = database.ROLES.Find(id);
            Permisos permission = database.Permisos.Find(permisoId);

            if (role.Permisos.Contains(permission))
            {
                role.Permisos.Remove(permission);
                database.SaveChanges();
            }
            return PartialView("_ListaRoles4Permisos", permission);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult AddRole2PermissionReturnPartialView(int permisoId, int roleId)
        {
            ROLES role = database.ROLES.Find(roleId);
            Permisos _permiso = database.Permisos.Find(permisoId);

            if (!role.Permisos.Contains(_permiso))
            {
                role.Permisos.Add(_permiso);
                database.SaveChanges();
            }
            return PartialView("_ListaRoles4Permisos", _permiso);
        }

        public ActionResult PermissionsImport()
        {            
            var _controllerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t != null
                    && t.IsPublic
                    && t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                    && !t.IsAbstract 
                    && typeof(IController).IsAssignableFrom(t));

            var _controllerMethods = _controllerTypes.ToDictionary(controllerType => controllerType,
                    controllerType => controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => typeof(ActionResult).IsAssignableFrom(m.ReturnType)));
            
            foreach (var _controller in _controllerMethods)
            {
                string _controllerName = _controller.Key.Name;
                foreach (var _controllerAction in _controller.Value)
                {
                    string _controllerActionName = _controllerAction.Name;
                    if (_controllerName.EndsWith("Controller"))
                    {
                        _controllerName = _controllerName.Substring(0, _controllerName.LastIndexOf("Controller"));
                    }

                    string _permissionDescription = string.Format("{0}-{1}", _controllerName, _controllerActionName);
                    Permisos _permiso = database.Permisos.Where(p => p.Descripcion == _permissionDescription).FirstOrDefault();
                    if (_permiso == null)
                    {
                        if (ModelState.IsValid)
                        {
                            Permisos _perm = new Permisos();
                            _perm.Descripcion = _permissionDescription;

                            database.Permisos.Add(_perm);
                            database.SaveChanges();
                        }
                    }
                }
            }
            return RedirectToAction("PermisosIndex");
        }
        #endregion
    }
}