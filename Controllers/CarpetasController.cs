using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace Requerimientos.Models
{
    public class CarpetasController : Controller
    {

        ILog log = LogManager.GetLogger(typeof(MensajesController));
        // GET: Mensajes
        private RequerimientosConn db = new RequerimientosConn();
        private int idusuario;
     //   private int? usuaridelegado;
     //   private int? estado;
        // GET: Carpetas
        public PartialViewResult Inicio()
        {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            var query = db.Carpetas.AsQueryable();
            query = query.Where(r => r.User_Id == idusuario);

            //  query = query.OrderByDescending(r => r.Fecha);


            return PartialView(query.ToList());
        }





        public ActionResult Create()
        {
            ViewBag.idusuario = new SelectList(db.Usuarios, "User_Id", "Usuario");
            return View();
        }

        // POST: Estado_novedades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Create(Carpetas carpetas, string mycolor)
        {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            if (ModelState.IsValid)
            {
                //carpetas.color = mycolor;
                carpetas.User_Id = idusuario;
                carpetas.Fecha = DateTime.Now;
                db.Carpetas.Add(carpetas);
                db.SaveChanges();
                return RedirectToAction("Inicio");
            }

            ViewBag.idusuario = new SelectList(db.Usuarios, "User_Id", "Usuario", carpetas.User_Id);
            return View(carpetas);
        }



        // GET: Estado_requerimiento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carpetas carpeta = db.Carpetas.Find(id);
            if (carpeta == null)
            {
                return HttpNotFound();
            }
            //TempData["error"] = "el estado se encuentra asociado a un mensaje";
            return View(carpeta);
        }

        // POST: Estado_requerimiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Carpetas carpeta = db.Carpetas.Find(id);
            try
            {



                db.Carpetas.Remove(carpeta);
                db.SaveChanges();
                TempData["success"] = "La carpeta se eliminó correctamente";
                return RedirectToAction("Inicio");

            }
            catch
            {
                TempData["error"] = "La carpeta tiene asociado/s mensaje/s";

            }

            return View(carpeta);


        }



        public ActionResult Borrar(int id)
        {
            Carpetas carpeta = db.Carpetas.Find(id);
            try
            {



                db.Carpetas.Remove(carpeta);
                db.SaveChanges();
                TempData["success"] = "La carpeta se eliminó correctamente";
                return RedirectToAction("Inicio");

            }
            catch
            {
                TempData["error"] = "La carpeta tiene asociado/s mensaje/s";

            }

            return RedirectToAction("Inicio");


        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }






    }
}