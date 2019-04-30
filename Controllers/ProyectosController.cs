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

namespace Requerimientos.Models
{
    [RBAC]
    public class ProyectosController : Controller
    {
        ILog log = LogManager.GetLogger(typeof(ProyectosController));
        private RequerimientosConn db = new RequerimientosConn();
        private int idusuario;

        // GET: Proyectos
        public ActionResult Index()
        {
            var proyectos = db.Proyectos.Include(p => p.Usuarios);
            return View(proyectos.ToList());
        }

        // GET: Proyectos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyectos proyectos = db.Proyectos.Find(id);
            if (proyectos == null)
            {
                return HttpNotFound();
            }
            ViewData["mensajeproyecto"] = db.Mensajes.Where(r => r.Idproyecto == id).Count();
            return View(proyectos);
        }

        // GET: Proyectos/Create
        public ActionResult Create()
        {
            ViewBag.CambioEstado = this.CambioEstado();
           
            return View();
        }

        // POST: Proyectos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Proyectos proyectos, string CambioEstado)
        {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            if (ModelState.IsValid)
            {
                proyectos.Estado = CambioEstado;
                proyectos.Fecha = DateTime.Now;

                proyectos.Creadopor = new SUIUsuarios(HttpContext.User.Identity.Name).Usuario;
                db.Proyectos.Add(proyectos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CambioEstado = this.CambioEstado();
            ViewBag.User_Id = new SelectList(db.Usuarios, "User_Id", "Usuario", proyectos.User_Id);
            return View(proyectos);
        }

        // GET: Proyectos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyectos proyectos = db.Proyectos.Find(id);
            if (proyectos == null)
            {
                return HttpNotFound();
            }
            ViewBag.CambioEstado = this.CambioEstado();
            ViewBag.User_Id = new SelectList(db.Usuarios, "User_Id", "Usuario", proyectos.User_Id);
            return View(proyectos);
        }

        // POST: Proyectos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Proyectos proyectos, string CambioEstado)
        {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            if (ModelState.IsValid)
            {
                db.Entry(proyectos).State = EntityState.Modified;
                proyectos.Fechamodif = DateTime.Now;
                proyectos.Estado = CambioEstado;
                proyectos.User_Id = idusuario;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_Id = new SelectList(db.Usuarios, "User_Id", "Usuario", proyectos.User_Id);
            return View(proyectos);
        }

        // GET: Proyectos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyectos proyectos = db.Proyectos.Find(id);
            if (proyectos == null)
            {
                return HttpNotFound();
            }
            return View(proyectos);
        }

        // POST: Proyectos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proyectos proyectos = db.Proyectos.Find(id);

            try
            {
               
                db.Proyectos.Remove(proyectos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                log.Error(ex.ToString());
            }
            return View(proyectos);
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        public List<SelectListItem> CambioEstado()
        {
            var _retVal = new List<SelectListItem>();
            try
            {
                _retVal.Add(new SelectListItem { Text = "Activo" });
                _retVal.Add(new SelectListItem { Text = "Inactivo" });
                _retVal.Add(new SelectListItem { Text = "Cerrado" });
            }
            catch { }
            return _retVal;
        }
    }
}
