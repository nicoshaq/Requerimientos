using log4net;
using Requerimientos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUI_Reportes.Controllers
{
    public class AvisoController : Controller
    {

        ILog log = LogManager.GetLogger(typeof(MensajesController));
        // GET: Mensajes
        private RequerimientosConn db = new RequerimientosConn();
        private int idusuario;
        //[RBAC]
        // GET: Aviso
        public PartialViewResult Index()
        {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            //var list = (from t in db.Mensajes
            //            where t.Estado == "Noleido" //&& t.Delivery.SentForDelivery == null
            //            orderby t.Fecha
            //            select t).OrderByDescending(o =>o.Fecha);


            var query = db.Mensajes//.Include(m => m.Usuarios)
              .AsQueryable();
            //query = query.Where(r => r.Proyectos.Estado == "Inactivo");
           
            query = query.Where(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);
            query = query.OrderByDescending(r => r.Fecha);






            int count = (from row in db.Mensajes
                         where row.Estado == "Noleido" && row.Idusuariodestino == idusuario 
                         select row).Count();

           // if (count) 


            ViewData["sinleer"] = count;

            // Response.AddHeader("Refresh", "5");
            return PartialView(query.Take(5));
        }
    }
}