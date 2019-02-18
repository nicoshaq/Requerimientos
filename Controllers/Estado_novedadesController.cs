using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Requerimientos.Models;
using log4net;
//using Requerimientos.Models;

//using Requerimientos.Models;

namespace Requerimientos.Controllers
{
    [RBAC]
    public class Estado_novedadesController : Controller
    {
        ILog log = LogManager.GetLogger(typeof(Estado_novedadesController));
        private RequerimientosConn db = new RequerimientosConn();
        private int idusuario;

        // GET: Estado_novedades
        public ActionResult Index()
        {
            var estado_novedades = db.Estado_novedades.Include(e => e.Usuarios);
            return View(estado_novedades.ToList());
        }

        // GET: Estado_novedades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado_novedades estado_novedades = db.Estado_novedades.Find(id);
            if (estado_novedades == null)
            {
                return HttpNotFound();
            }
            return View(estado_novedades);
        }

        // GET: Estado_novedades/Create
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
        public ActionResult Create(Estado_novedades estado_novedades, string mycolor)
        {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            if (ModelState.IsValid)
            {
                estado_novedades.color = mycolor;
                estado_novedades.idusuario = idusuario;
                db.Estado_novedades.Add(estado_novedades);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idusuario = new SelectList(db.Usuarios, "User_Id", "Usuario", estado_novedades.idusuario);
            return View(estado_novedades);
        }

        // GET: Estado_novedades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado_novedades estado_novedades = db.Estado_novedades.Find(id);
            if (estado_novedades == null)
            {
                return HttpNotFound();
            }
            ViewBag.idusuario = new SelectList(db.Usuarios, "User_Id", "Usuario", estado_novedades.idusuario);
            return View(estado_novedades);
        }

        // POST: Estado_novedades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idestadonovedad,Descripcion,idusuario")] Estado_novedades estado_novedades)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estado_novedades).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idusuario = new SelectList(db.Usuarios, "User_Id", "Usuario", estado_novedades.idusuario);
            return View(estado_novedades);
        }

        // GET: Estado_novedades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado_novedades estado_novedades = db.Estado_novedades.Find(id);
            if (estado_novedades == null)
            {
                return HttpNotFound();
            }

            return View(estado_novedades);
        }

        // POST: Estado_novedades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estado_novedades estado_novedades = db.Estado_novedades.Find(id);


            try
            {
                db.Estado_novedades.Remove(estado_novedades);
                db.SaveChanges();
                TempData["success"] = "el estado se eliminó correctamente";
                return RedirectToAction("Index");


            }
            catch (Exception ex)
            {
                log.ErrorFormat(ex.ToString());
                TempData["error"] = "el estado se encuentra asociado a un mensaje";
            }
          
            return View(estado_novedades);

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
