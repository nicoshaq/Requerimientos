using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Requerimientos.Models;
using System.IO;


namespace Requerimientos.Models
{
    [RBAC]
    public class NovedadesController : Controller
    {
        private RequerimientosConn db = new RequerimientosConn();
        private int idusuario;

        // GET: Novedades
        public ActionResult Index()
        {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            var hola = new Novedades();
            //var novedades = db.Novedades.Include(n => n.Mensajes);

            if (hola.Privado)
            {
                var query = db.Novedades.Include(m => m.Mensajes).AsQueryable();
               
                query = query.Where(r => r.User_Id == idusuario && r.Privado);
                query = query.OrderByDescending(r => r.Fecha);

                return View(query.ToList());

            }
            else
            {
                var novedades = db.Novedades.Include(n => n.Mensajes);
                return View(novedades.ToList());
            }


          
        }

        // GET: Novedades/Details/5
        public ActionResult Details(int? id)
        {

            ViewBag.CambioEstado = this.CambioEstado();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Novedades novedades = db.Novedades.Find(id);
            if (novedades == null)
            {
                return HttpNotFound();
            }
            return View(novedades);
        }




        [HttpPost]
        [ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult Details(Novedades support, string CambioEstado)
        {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            if (ModelState.IsValid)
            {



                db.Entry(support).State = EntityState.Modified;
                //support.User_Id = idusuario;
                support.Estado = CambioEstado;

                db.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(support);
        }















        // GET: Novedades/Create
        public ActionResult Create(int? id, int? idproyecto)
        {
            
            if (id == null) {
                ViewBag.ocultar = false;
                ViewBag.CambioEstado = this.CambioEstado();
                ViewBag.Idmensaje = new SelectList(db.Mensajes, "Id", "Asunto");
                
              
            }
            else
            {
                ViewBag.CambioEstado = this.CambioEstado();
                ViewBag.ocultar = true;
            }
           
            return View();
        }

        // POST: Novedades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
       // [ValidateAntiForgeryToken]
        public ActionResult Create(Novedades novedades, int? id, int? idproyecto, int? CambioEstado, int? Idmensaje)
        {

          //  idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;

            if (ModelState.IsValid)
            {



                if (id ==  null) {
                    novedades.Idmensaje = Idmensaje;
                }
                else
                {
                    novedades.Idmensaje = id;

                }
                
                var query = from c in db.Mensajes
                                where c.Id == id
                                select new
                                {
                                    c.Idproyecto

                                };
                foreach (var c in query)
                {
                    novedades.Idproyecto = c.Idproyecto;

                }

                List<Archivos> fileDetails = new List<Archivos>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        Archivos fileDetail = new Archivos()
                        {
                            Nombre = DateTime.Now + "_" + fileName,
                            Extension = Path.GetExtension(fileName),
                            Id = Guid.NewGuid(),
                            Fecha = DateTime.Now,
                            Idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id

                        };
                        fileDetails.Add(fileDetail);

                        var path = Path.Combine(Server.MapPath("~/Content/Upload/"), fileDetail.Id + fileDetail.Extension);
                        file.SaveAs(path);
                    }
                }



                novedades.Fecha = DateTime.Now;
                novedades.Usuario =  new SUIUsuarios(HttpContext.User.Identity.Name).Usuario;


                novedades.Archivos = fileDetails;
                novedades.Idestadonovedad = CambioEstado;
                novedades.User_Id = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
                if (novedades.Privado)
                {
                    novedades.Privado = true;
                }
                else
                {
                    novedades.Privado = false;
                }


                db.Novedades.Add(novedades);
                db.SaveChanges();
                if (id == null)
                {
                    return View(novedades);
                }
                else
                {
                    TempData["success"] = "La novedad que creo se encuentra "+ CambioEstado;
                    return RedirectToAction("Details"+"/" +id, "Mensajes");
                   // return RedirectToAction("/Mensajes/Details/" + id);
                }
             
            }
            ViewBag.CambioEstado = this.CambioEstado();
            ViewBag.Idmensaje = new SelectList(db.Mensajes, "Id", "Asunto");
            
            return View(novedades);
        }

        // GET: Novedades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Novedades novedades = db.Novedades.Find(id);
            if (novedades == null)
            {
                return HttpNotFound();
            }
            ViewBag.Idmensaje = new SelectList(db.Mensajes, "Id", "Asunto", novedades.Idmensaje);
            ViewBag.CambioEstado = this.CambioEstado();
            return View(novedades);
        }

        // POST: Novedades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Novedades novedades, string CambioEstado)
        {
            if (ModelState.IsValid)
            {
                novedades.Estado = CambioEstado;
                db.Entry(novedades).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Idmensaje = new SelectList(db.Mensajes, "Id", "Asunto", novedades.Idmensaje);
            ViewBag.CambioEstado = this.CambioEstado();
            return View(novedades);
        }

        // GET: Novedades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Novedades novedades = db.Novedades.Find(id);
            if (novedades == null)
            {
                return HttpNotFound();
            }
            return View(novedades);
        }

        // POST: Novedades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Novedades novedades = db.Novedades.Find(id);
            db.Novedades.Remove(novedades);
            db.SaveChanges();
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


       





        public List<SelectListItem> CambioEstado()
        {

            var content = from p in db.Estado_novedades
                              //here p.Inactivo == false
                          select new { p.idestadonovedad, p.Descripcion };

            ViewBag.novedad = content
                .Select(c => new SelectListItem
                {
                    Text = c.Descripcion,
                    Value = c.idestadonovedad.ToString()
                }).ToList();

            return ViewBag.novedad;
            //var _retVal = new List<SelectListItem>();
            //try
            //{
            //    _retVal.Add(new SelectListItem { Text = "Iniciado" });
            //    _retVal.Add(new SelectListItem { Text = "Proceso" });
            //    _retVal.Add(new SelectListItem { Text = "Finalizado" });
            //}
            //catch { }
            //return _retVal;
        }








    }
}
