using log4net;
using Requerimientos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Requerimientos.Controllers
{
    public class AvisoController : Controller
    {

        ILog log = LogManager.GetLogger(typeof(MensajesController));
       
        private RequerimientosConn db = new RequerimientosConn();
        private int idusuario;
        //[RBAC]
    
        public PartialViewResult Index()
        {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
          
            var query = db.Mensajes.AsQueryable();           
            query = query.Where(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);
            query = query.Where(r => r.Estado == "Noleido");
            query = query.OrderByDescending(r => r.Fecha);

            
            int count = (from row in db.Mensajes
                         where row.Estado == "Noleido" && row.Idusuariodestino == idusuario 
                         select row).Count();

            ViewData["sinleer"] = count;

            
            return PartialView(query.Take(5));
        }
    }
}