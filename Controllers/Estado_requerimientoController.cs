using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Requerimientos.Models;

namespace Requerimientos.Controllers
{
    [RBAC]
    public class Estado_requerimientoController : Controller
    {
        private RequerimientosConn db = new RequerimientosConn();
        private int idusuario;

        // GET: Estado_requerimiento
        public ActionResult Index()
        {
            var estado_requerimiento = db.Estado_requerimiento.Include(e => e.Usuarios);
            return View(estado_requerimiento.ToList());
        }

        // GET: Estado_requerimiento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado_requerimiento estado_requerimiento = db.Estado_requerimiento.Find(id);
            if (estado_requerimiento == null)
            {
                return HttpNotFound();
            }
            return View(estado_requerimiento);
        }

        // GET: Estado_requerimiento/Create
        public ActionResult Create()
        {
            ViewBag.idusuario = new SelectList(db.Usuarios, "User_Id", "Usuario");
            return View();
        }

        // POST: Estado_requerimiento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Estado_requerimiento estado_requerimiento, string mycolor)
        {


            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;

            if (ModelState.IsValid)
            {
                var query = db.Usuarios
              .Include(p => p.Area).AsQueryable();
               
                query = query.Where(r => r.User_Id == idusuario);
                int Idarearequerimiento = Convert.ToInt32(query.FirstOrDefault().Idarea);

                estado_requerimiento.Idearea = Idarearequerimiento;
                estado_requerimiento.color = mycolor;
                estado_requerimiento.idusuario = idusuario;
                db.Estado_requerimiento.Add(estado_requerimiento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idusuario = new SelectList(db.Usuarios, "User_Id", "Usuario", estado_requerimiento.idusuario);
            return View(estado_requerimiento);
        }

        // GET: Estado_requerimiento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado_requerimiento estado_requerimiento = db.Estado_requerimiento.Find(id);
            if (estado_requerimiento == null)
            {
                return HttpNotFound();
            }
           
            return View(estado_requerimiento);
        }

        // POST: Estado_requerimiento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Estado_requerimiento estado_requerimiento, string mycolor)
        {

            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            if (ModelState.IsValid)
            {

                db.Entry(estado_requerimiento).State = EntityState.Modified;
                estado_requerimiento.color = mycolor;
                estado_requerimiento.idusuario = idusuario;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idusuario = new SelectList(db.Usuarios, "User_Id", "Usuario", estado_requerimiento.idusuario);
            return View(estado_requerimiento);
        }

        // GET: Estado_requerimiento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado_requerimiento estado_requerimiento = db.Estado_requerimiento.Find(id);
            if (estado_requerimiento == null)
            {
                return HttpNotFound();
            }
           
            return View(estado_requerimiento);
        }

        // POST: Estado_requerimiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estado_requerimiento estado_requerimiento = db.Estado_requerimiento.Find(id);
            try
            {


               
                db.Estado_requerimiento.Remove(estado_requerimiento);
                db.SaveChanges();
                TempData["success"] = "el estado se eliminó correctamente";
                return RedirectToAction("Index");

            }
            catch
            {
                TempData["error"] = "el estado se encuentra asociado a un mensaje";

            }

            return View(estado_requerimiento);


        }

        public ActionResult Borrar(int id)
        {
            Estado_requerimiento estado_requerimiento = db.Estado_requerimiento.Find(id);
            try
            {



                db.Estado_requerimiento.Remove(estado_requerimiento);
                db.SaveChanges();
                TempData["success"] = "El estado de requerimiento se eliminó correctamente";
                return RedirectToAction("Index");

            }
            catch
            {
                TempData["error"] = "El estado tiene asociado/s requerimiento/s";

            }

            return RedirectToAction("Index");


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
