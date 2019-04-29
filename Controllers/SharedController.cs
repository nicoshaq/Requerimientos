using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Requerimientos.Models
{
    [RBAC]
    public class SharedController : Controller
    {

        ILog log = LogManager.GetLogger(typeof(MensajesController));
        // GET: Mensajes
        private RequerimientosConn db = new RequerimientosConn();

        [RBAC]


      
        public ActionResult _Layout_2()
        {
            return View();
        }




        public PartialViewResult Avisomensaje()
        {



            int count = (from row in db.Mensajes
                         where row.Estado == "Noleido"
                         select row).Count();

            ViewData["sinleer"] = count;

            //Response.AddHeader("Refresh", "5");

            return PartialView();
        }


    }
}





