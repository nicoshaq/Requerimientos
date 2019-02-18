using RBACModel;
using Requerimientos.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using log4net;
using System.Net.Mail;

namespace Requerimientos.Models
{
    [RBAC]
    public class MensajesController : Controller
    {
        
        ILog log = LogManager.GetLogger(typeof(MensajesController));
        // GET: Mensajes
        private RequerimientosConn db = new RequerimientosConn();
        private int idusuario;
        private int? usuaridelegado;
        private int? estado;
        MailMessage mail = new MailMessage();
        private string proyecto;

        protected override void Dispose(bool disposing)
        {
            db.Dispose();

           // log.Error(disposing);
            base.Dispose(disposing);
        }

     

        public ActionResult Index()
        {

              var fechainicio = DateTime.Today.AddDays(-365);
           
        idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            ViewData["contarmensajeentrada"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);

            ViewData["contarmensajesalida"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino != idusuario);


            ViewData["Aceptado"] = db.Mensajes.Count(r => r.Status == "Aceptado");
            ViewData["Rechazado"] = db.Mensajes.Count(r => r.Status == "Rechazado");
            ViewData["Inconsistente"] = db.Mensajes.Count(r => r.Status == "Incosistente");


            ViewBag.IdCarpeta = this.CarpetasCombo();

            var query = db.Mensajes//.Include(m => m.Usuarios)
                .Include(p =>p.Proyectos).AsQueryable();
            //query = query.Where(r => r.Proyectos.Estado == "Inactivo");
            query = query.Where(r => r.IdCarpeta == 1 || r.IdCarpeta == null);
            query = query.Where(r => r.Fecha >= fechainicio);
            query = query.Where(r => r.Fecha <= DateTime.Now);
            query = query.Where(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);
            query = query.OrderByDescending(r => r.Fecha);

//            this.hola();
          




            return View(query.ToList());



            //var list_join = (from a in db.Mensajes.Include(m => m.Usuarios)
                            
            //                 where a.Idusuariodestino == idusuario || a.Idusuariodelega == idusuario
            //      select a).ToList();
            //log.Info("mensaje de log");
            //return View(list_join);

        
            //var mensajes = db.Mensajes.Include(m => m.USERS);
            // return View(db.Mensajes.Include(m => m.Usuarios).Where(r => r.Idusuariodestino == idusuario).ToList());
            // return View(db.Mensajes.ToList());
        }



        //public PartialViewResult Entrada()
        //{

        //    var fechainicio = DateTime.Today.AddDays(-5);

        //    idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
        //    ViewData["contarmensajeentrada"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);

        //    ViewData["contarmensajesalida"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino != idusuario);


        //    ViewData["Aceptado"] = db.Mensajes.Count(r => r.Status == "Aceptado");
        //    ViewData["Rechazado"] = db.Mensajes.Count(r => r.Status == "Rechazado");
        //    ViewData["Inconsistente"] = db.Mensajes.Count(r => r.Status == "Incosistente");




        //    var query = db.Mensajes.Include(m => m.Usuarios).AsQueryable();
        //    query = query.Where(r => r.Fecha >= fechainicio);
        //    query = query.Where(r => r.Fecha <= DateTime.Now);
        //    query = query.Where(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);
        //    query = query.OrderByDescending(r => r.Fecha);


        //    return PartialView(query.ToList());



        //    //var list_join = (from a in db.Mensajes.Include(m => m.Usuarios)

        //    //                 where a.Idusuariodestino == idusuario || a.Idusuariodelega == idusuario
        //    //      select a).ToList();
        //    //log.Info("mensaje de log");
        //    //return View(list_join);


        //    //var mensajes = db.Mensajes.Include(m => m.USERS);
        //    // return View(db.Mensajes.Include(m => m.Usuarios).Where(r => r.Idusuariodestino == idusuario).ToList());
        //    // return View(db.Mensajes.ToList());
        //}



        //[HttpPost]
        //public PartialViewResult Entrada(string start, string end)
        //{

        //    idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;


        //    //RequerimientosConn db = new RequerimientosConn();
        //    RequerimientosConn db = new RequerimientosConn();
        //    ViewData["contarmensajeentrada"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);

        //    ViewData["contarmensajesalida"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino != idusuario);

        //    ViewData["Aceptado"] = db.Mensajes.Count(r => r.Status == "Aceptado");
        //    ViewData["Rechazado"] = db.Mensajes.Count(r => r.Status == "Rechazado");
        //    ViewData["Inconsistente"] = db.Mensajes.Count(r => r.Status == "Incosistente");
        //    if (start == "")
        //    {
        //        TempData["error"] = "Debe seleccionar una fecha";
        //        return PartialView("Entrada");

        //    }
        //    else if (end == "")
        //    {
        //        TempData["error"] = "Debe seleccionar una fecha";
        //        return PartialView("Entrada");
        //    }
        //    else if (DateTime.ParseExact(start, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat) > DateTime.ParseExact(end, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat))
        //    {
        //        TempData["error"] = "Fecha Hasta debe ser mayor";
        //        return PartialView("Entrada");
        //    }
        //    else
        //    {

        //        // var maniana = DateTime.Today.AddDays(-19);
        //        DateTime starter = DateTime.ParseExact(start, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
        //        DateTime ender = DateTime.ParseExact(end, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);


        //        //TempData["warning"] = "mensaje de warning!!";
        //        //TempData["success"] = "mensaje de success!!";
        //        //TempData["info"] = "mensaje de informacion!!";
        //        //TempData["error"] = "mensaje de error!!";

        //        var query = db.Mensajes.Include(m => m.Usuarios).AsQueryable();
        //        query = query.Where(r => r.Fecha >= starter);
        //        query = query.Where(r => r.Fecha <= ender);
        //        query = query.Where(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);
        //        query = query.OrderByDescending(r => r.Fecha);


        //        // return View(query.ToList());

        //        int count = (from row in db.Mensajes
        //                     where row.Fecha >= starter && row.Fecha <= ender
        //                     select row).Count();
        //        TempData["info"] = "Se encontraron " + count + " registros";

        //        return PartialView("Entrada",query.ToList());

        //    }





        //}



        [HttpPost]
        public ActionResult Index(Mensajes support, string start, string end, IList<int> deleteInputs, string submit)
        {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;

            RequerimientosConn db = new RequerimientosConn();

            switch (submit)
            {
                case "Cambiar":

                    if (support.IdCarpeta != null)
                    {

                        if (deleteInputs != null)
                        {
                            var carpe = (from b in db.Carpetas

                                          where b.Id == support.IdCarpeta
                                         select b.Nombre).FirstOrDefault();
                            

                            if (deleteInputs != null || support.IdCarpeta != null)
                            {
                                foreach (var item in deleteInputs)
                                {
                                    

                                    try
                                    {
                                       

                                        db.AgruparCarpetas(item, support.IdCarpeta);
                                        //support.Id = deleteInputs;
                                        //support.IdCarpeta = support.IdCarpeta;
                                        //db.Entry(support).State = EntityState.Modified;

                                        //db.SaveChanges();

                                    }
                                    catch (Exception ex)
                                    {

                                        log.Error(ex.ToString());
                                        // return RedirectToAction("Index");
                                    }
                                }
                                TempData["info"] = "El/los mensaje/s se agregaron a la carpeta " + carpe;
                                log.InfoFormat("Los mensajes se agregaron a {0}", carpe);
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            TempData["error"] = "No hay mensajes seleccionados";
                            log.Warn("No hay mensajes seleccionados para enviar a la carpeta");
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["error"] = "No hay carpeta de destino seleccionada";
                        log.Warn("No hay carpeta de destino seleccionada para los mensajes");
                        return RedirectToAction("Index");
                    }

                    break;

                        default:





                    //foreach (var item in )
                    //{

                    //}
                    //(int i = 0; i < Request.Files.Count; i++)
                    //{
                    //    var file = Request.Files[i];

                    //    if (file != null && file.ContentLength > 0)
                    //    {

                    //    }
                    //}
                    // var hola = deleteInputs;

                    //RequerimientosConn db = new RequerimientosConn();

                    ViewData["contarmensajeentrada"] =
                        db.Mensajes.Include(m => m.Usuarios)
                            .Count(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);

                    ViewData["contarmensajesalida"] =
                        db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino != idusuario);

                    ViewData["Aceptado"] = db.Mensajes.Count(r => r.Status == "Aceptado");
                    ViewData["Rechazado"] = db.Mensajes.Count(r => r.Status == "Rechazado");
                    ViewData["Inconsistente"] = db.Mensajes.Count(r => r.Status == "Incosistente");

                    ViewBag.IdCarpeta = this.CarpetasCombo();

                    if (start == "")
                    {
                        TempData["error"] = "Debe seleccionar una fecha";
                        return Index();

                    }
                    else if (end == "")
                    {
                        TempData["error"] = "Debe seleccionar una fecha";
                        return Index();
                    }
                    else if (
                        DateTime.ParseExact(start, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat) >
                        DateTime.ParseExact(end, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat))
                    {
                        TempData["error"] = "Fecha Hasta debe ser mayor";
                        return Index();
                    }
                    else
                    {

                        // var maniana = DateTime.Today.AddDays(-19);
                        DateTime starter = DateTime.ParseExact(start, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                        DateTime ender = DateTime.ParseExact(end, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);


                        //TempData["warning"] = "mensaje de warning!!";
                        //TempData["success"] = "mensaje de success!!";
                        //TempData["info"] = "mensaje de informacion!!";
                        //TempData["error"] = "mensaje de error!!";

                        var query = db.Mensajes.Include(m => m.Usuarios).AsQueryable();
                        query = query.Where(r => r.IdCarpeta == 1);
                        query = query.Where(r => r.Fecha >= starter);
                        query = query.Where(r => r.Fecha <= ender);
                        query = query.Where(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);
                        query = query.OrderByDescending(r => r.Fecha);



                        // return View(query.ToList());
                        ViewData["Fechas"] = "Periodo entre: " + start + " y " + end;
                        int count = (from row in db.Mensajes
                                     where row.Fecha >= starter && row.Fecha <= ender
                                     select row).Count();
                        TempData["info"] = "Se encontraron " + count + " registros";

                        return View(query.ToList());

                    }


            }



            return RedirectToAction("Index");








        }


        public ActionResult Edit(int? id)
      {
          if (id == null)
          {
              return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
          }
            



            ViewBag.CambioEstado = this.CambioEstado();
            Mensajes support = db.Mensajes.Include(s => s.Archivos).SingleOrDefault(x => x.Id == id);
          if (support == null)
          {
              return HttpNotFound();
          }
          return View(support);
      }
       
   [HttpPost]
      [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Mensajes support, string Status)
      {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            if (ModelState.IsValid)
          {
             

                  


                //New Files
                for (int i = 0; i < Request.Files.Count; i++)
              {
                  var file = Request.Files[i];
 
                  if (file != null && file.ContentLength > 0)
                  {
                      var fileName = Path.GetFileName(file.FileName);
                      Archivos fileDetail = new Archivos()
                      {
                          Nombre = fileName,
                          Extension = Path.GetExtension(fileName),
                          Id = Guid.NewGuid(),
                          Idmensaje = support.Id
                      };
                      var path = Path.Combine(Server.MapPath("~/Content/Upload/"), fileDetail.Id + fileDetail.Extension);
                      file.SaveAs(path);
 
                      db.Entry(fileDetail).State = EntityState.Added;
                  }
              }
 
              db.Entry(support).State = EntityState.Modified;
                support.User_Id = idusuario;
                support.Status = Status;

              db.SaveChanges();
              return RedirectToAction("Index");
          }
          return View(support);
      }



        public ActionResult Create()
        {

            //if (this.HasRole("Administrador"))
            //{

            //    ViewBag.User_Id = new SelectList(db.Usuarios, "User_Id", "Usuario");
            //}
            //else
            //{
            //    ViewBag.User_Id = new SelectList(db.Usuarios, "User_Id", "Usuario", 3);

            //    ViewBag.disable = true;

            //}
            //ViewBag.User_Id = new SelectList(db.Usuarios, "User_Id", "Usuario");


            ViewBag.Idproyecto = this.ProyectoCombo(); //new SelectList(db.Proyectos, "Idproyecto", "Titulo");

            ViewBag.User_Id = this.Lista();


            //var content = from p in db.Usuarios
            //              where p.Inactivo == false
            //              select new { p.User_Id, p.Usuario };

            //ViewBag.User_Id = content
            //    .Select(c => new SelectListItem
            //    {
            //        Text = c.Usuario,
            //        Value = c.User_Id.ToString()
            //    }).ToList();

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Mensajes support, int User_Id, int Idproyecto, string txtTo, string txtSubject, string txtMessage, string txtTipoproyecto, string txtRemitente, string txtDestinatario)
        {


            try
            {

                if (ModelState.IsValid)
                {
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
                                Idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id,
                                Idproyecto = Idproyecto


                            };
                            fileDetails.Add(fileDetail);

                            var path = Path.Combine(Server.MapPath("~/Content/Upload/"), fileDetail.Id + fileDetail.Extension);
                            file.SaveAs(path);
                        }
                    }

                    var query_ne1 = from c in db.Usuarios
                                    where c.User_Id == User_Id
                                    select new
                                    {
                                        c.Nombre,
                                        c.Usuario,
                                        c.User_Id,
                                        c.EMail
                                    };
                    foreach (var c in query_ne1)
                    {
                        support.Destinatario = c.Nombre;
                        support.Idusuariodestino = c.User_Id;
                        support.Maildestino = c.EMail;
                        txtTo = c.EMail;
                    }



                    var query_ne2 = from P in db.Proyectos
                                    where P.Idproyecto == Idproyecto
                                    select new
                                    {
                                       
                                        P.Titulo
                                    };
                    foreach (var P in query_ne2)
                    {
                         proyecto = P.Titulo;
                    }




                    var usus = db.Usuarios
                                 .Where(g => g.User_Id == User_Id)
                                .Select(g => g.Nombre).ToList();

                    // var groups = db.Usuarios
                    //.Where(g => g.Usuario.Contains(g.User_Id));
                    support.IdCarpeta = 1;
                    support.Idproyecto = Idproyecto;
                    support.Fecha = DateTime.Now;
                    support.User_Id = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
                    support.Remitente = new SUIUsuarios(HttpContext.User.Identity.Name).Usuario;
                    support.Estado = "Noleido";


                    support.Archivos = fileDetails;
                    db.Mensajes.Add(support);
                    db.SaveChanges();

                    //Para enviar mail al destinatario avisando que recibio un nuevo requerimiento
                    if (support.Enviamail)
                    {
                        //declaro los datos que se van a enviar al mail

                        //string asunto = support.Asunto + " - Requerimientos";
                        //string remitente = "Recibió un requerimiento de: " + support.Remitente + "\n";
                        //string tipoproyecto = "Proyecto: " + proyecto + "\n";
                        //string mensaje = support.Mensaje + "\n";



                        string asunto = support.Asunto + " - Requerimientos";
                        string remitente =support.Remitente;
                        string tipoproyecto = proyecto;
                        string mensaje = support.Mensaje;
                        string destinatario = support.Destinatario;

                        string[] to = txtTo.Split(';');
                        txtSubject = asunto; //"alalalalal";
                        //txtMessage = remitente + tipoproyecto + mensaje; //"jajajaajajajaj";

                        txtTipoproyecto = tipoproyecto;
                            txtRemitente = remitente;
                        txtDestinatario = destinatario;
                            txtMessage =  mensaje;
                        foreach (string emailAdd in to)
                        {

                            if (!string.IsNullOrEmpty(emailAdd))
                                SendEmail(emailAdd, txtSubject, txtMessage, txtTipoproyecto, txtRemitente, txtDestinatario);

                          //  this.SendHtmlFormattedEmail("New article published!", SendEmail);
                        }
                       

                    }

                 

                    // txtTo = c.ToString();
                  

                    TempData["success"] = "El Requerimiento se envió correctamente";
                    if (support.Enviamail)
                    {
                        TempData["success"] = "El Requerimiento se envió correctamente con el Email de notificación";
                        log.Info("Se envio una notificacion al email");

                    }

                 
                    log.Info("Se creo un nuevo requerimiento");
                    return RedirectToAction("Index");
                }
            }catch (Exception ex)
                {
                log.Error(ex.ToString());
            }

            return View(support);
        }


       




        public ActionResult Details(int? id)
        {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            Mensajes mensajes = db.Mensajes.Find(id);

            if (this.HasRole("Administrador"))
            {

                ViewBag.ocultar = false;

            }
            else
            {

                ViewBag.ocultar = true;
            }

            //if (mensajes.User_Id== idusuario)
            //{
            //    ViewBag.ocultar = true;
            //    ViewBag.estadodos = true;
            //}


            if(mensajes.Proyectos.Estado == "Activo")
            {
                ViewBag.estadodos = false;
            }
            else
            {
                ViewBag.estadodos = true;
            }



            usuaridelegado = mensajes.Idusuariodelega;

          

            if (mensajes.Idusuariodelega != null) {
                ViewBag.userdelega = new SelectList(db.Usuarios, "User_Id", "Nombre", usuaridelegado);
            }
            else
            {
                ViewBag.userdelega = new SelectList(db.Usuarios, "User_Id", "Nombre");
            }


            estado = mensajes.Idestado;

            if (mensajes.Idestado !=null) {
                ViewBag.CambioEstado = new SelectList(db.Estado_requerimiento, "Idestado", "Descripcion", estado);

            }else
            {
                ViewBag.CambioEstado = this.CambioEstado();
            }

           


            if (mensajes == null)
            {
                return HttpNotFound();
            }
            if (mensajes.Estado == "leido")
            {
                return View(mensajes);

            }

            if (mensajes.Idusuariodestino == idusuario || mensajes.Idusuariodelega == idusuario) {
                mensajes.Estado = "leido";
                db.Entry(mensajes).State = EntityState.Modified;
                db.SaveChanges();

            }

          
          
           
            return View(mensajes);
        }

       

        [HttpPost]
        [ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult Details(Mensajes support, int? userdelega, int? CambioEstado)
        {

            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;


            var delegacion = (from a in db.Mensajes

                             where a.Id == support.Id
                        select new { a.Idusuariodelega, a.Usuariodelega }).FirstOrDefault();

        

            var estado = (from b in db.Mensajes

                        where b.Id == support.Id
                        select b.Idestado).FirstOrDefault();

            //if(User_Id == null)
            //{

            //    User_Id = 0;
            //}

            try { 


            if (ModelState.IsValid)
            {


                if (userdelega != delegacion.Idusuariodelega) {
                    var query_ne1 = from c in db.Usuarios
                                    where c.User_Id == userdelega
                                    select new
                                    {

                                        c.Nombre

                                    };
                    foreach (var c in query_ne1)
                    {
                        support.Usuariodelega = c.Nombre;
                        TempData["success"] = "Ha delegado el mensaje a " + c.Nombre;
                        support.Idusuariodelega = userdelega;
                            log.Info("la delegacion de usuario se realizo con exito");
                    }
                   // db.Entry(support).State = EntityState.Modified;
                    support.User_Id = idusuario;


                        HistorialDelega historico = new HistorialDelega()
                        {
                           Idmensaje = support.Id,
                           Idproyecto = support.Idproyecto,
                           Usuariodelega = support.Destinatario,
                           Usuariodelegado = support.Usuariodelega
                           
                        };

                        if(historico.Usuariodelegado != null)
                        {


                            // SP que inserta en la tabla HistorialDelega los usuarios delegados
                            db.historialDelegacion(historico.Idmensaje, historico.Idproyecto, historico.Usuariodelega, historico.Usuariodelegado);

                            log.Info("Se guardó el usuario delegado en el historial");

                        }

                    }
                else
                {

                    support.Idusuariodelega = delegacion.Idusuariodelega;
                    support.Usuariodelega = delegacion.Usuariodelega;
                    //TempData["info"] = "lalalalalalal";

                    log.Warn("Delegar Usuario no se modifica");
                }





                           








                if ( CambioEstado != estado)
                {
                    support.Idestado = CambioEstado;
                    TempData["success"] = "Ha cambiado el estado del mensaje a " + CambioEstado;
                        log.Info("El cambio de estado se realizo con exito");
                    }
                else
                {
                    support.Idestado = estado;
                    log.Warn("El Estado del requerimiento no se modifica");
                   // TempData["error"] = "Ha eliminado una delegacion o no selecciono usuario";
                }

                db.Entry(support).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Details");
            }

            }catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            ViewBag.userdelega = new SelectList(db.Usuarios, "User_Id", "Usuario", usuaridelegado);
            return View(support);
        }






        public ActionResult Enviados()
        {
            
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;

            ViewData["contarmensajeentrada"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);

            ViewData["contarmensajesalida"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino != idusuario);
            //var mensajes = db.Mensajes.Include(m => m.USERS);
            return View(db.Mensajes.Include(m => m.Usuarios).Where(r => r.Idusuariodestino != idusuario).ToList());
            // return View(db.Mensajes.ToList());
        }






        //public ActionResult Lista()
        //{
        //    var model = new Usuarios();

        //    using (var dc = new RequerimientosConn())
        //    {
        //        var content = from p in db.Usuarios
        //                      select new { p.User_Id, p.Usuario };

        //        model.= content
        //            .Select(c => new SelectListItem
        //            {
        //                Text = c.Usuario,
        //                Value = c.User_Id.ToString()
        //            }).ToList();
        //    }

        //    return View(model);
        //}



        public FileResult Descarga(String p, String d)
        {
            return File(Path.Combine(Server.MapPath("~/Content/Upload/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }


        public List<SelectListItem> CambioEstado()
        {

            var content = from p in db.Estado_requerimiento
                         //here p.Inactivo == false
                          select new { p.Idestado, p.Descripcion };

            ViewBag.estado = content
                .Select(c => new SelectListItem
                {
                    Text = c.Descripcion,
                    Value = c.Idestado.ToString()
                }).ToList();

            return ViewBag.estado;
            //var _retVal = new List<SelectListItem>();
            //try
            //{
            //    _retVal.Add(new SelectListItem { Text = "Aceptado"});
            //    _retVal.Add(new SelectListItem { Text = "Incosistente"});
            //    _retVal.Add(new SelectListItem { Text = "Rechazado" });
            //}
            //catch { }
            //return _retVal;
        }



        public List<SelectListItem> Lista()
        {
            var content = from p in db.Usuarios
                          where p.Inactivo == false
                          select new { p.User_Id, p.Nombre };

            ViewBag.usuario = content
                .Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.User_Id.ToString()
                }).ToList();

            return ViewBag.usuario;
        }




        public List<SelectListItem> ProyectoCombo()
        {
            var content = from p in db.Proyectos
                          where p.Estado == "activo"
                          select new { p.Idproyecto, p.Titulo };

            ViewBag.proyectos = content
                .Select(c => new SelectListItem
                {
                    Text = c.Titulo,
                    Value = c.Idproyecto.ToString()
                }).ToList();

            return ViewBag.proyectos;
        }


        public List<SelectListItem> Error()
        {
            string error = ModelState
                .First(x => x.Value.Errors.Count > 0)
                .Value
                .Errors[0]
                .ErrorMessage;

            ViewBag.errores = error
               .Select(c => new SelectListItem
               {
                   Text = c.ToString()
                 
               }).ToList();

            return ViewBag.errores;
            // return Error();


        }







        #region Carpetas


        public List<SelectListItem> CarpetasCombo()
        {
            var content = from p in db.Carpetas
                          where p.User_Id == idusuario || p.Fecha == null
                          select new { p.Id, p.Nombre };

            ViewBag.Carpetas = content
                .Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                }).ToList();

            return ViewBag.Carpetas;
        }


        public PartialViewResult Inicios()
        {

            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            int noasignado = 0;

            var proyectos = db.Carpetas.Include(p => p.Mensajes).AsQueryable();

            proyectos = proyectos.Where(r => r.User_Id == idusuario);

            var conteo = db.Mensajes.Include(r => r.Mensaje).Count();

            ViewData["conteo"] = "(" + conteo + ")";
            return PartialView(proyectos.ToList());
            //var query = db.Carpetas.AsQueryable();

            //  query = query.OrderByDescending(r => r.Fecha);


            //return PartialView(query.ToList());
        }

        // GET: Proyectos/Details/5
        public ActionResult Carpetadetalle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var fechainicio = DateTime.Today.AddDays(-365);

            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            ViewData["contarmensajeentrada"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);

            ViewData["contarmensajesalida"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino != idusuario);


            ViewData["Aceptado"] = db.Mensajes.Count(r => r.Status == "Aceptado");
            ViewData["Rechazado"] = db.Mensajes.Count(r => r.Status == "Rechazado");
            ViewData["Inconsistente"] = db.Mensajes.Count(r => r.Status == "Incosistente");

            Carpetas proyectos = db.Carpetas.Find(id);
         
           

            var conteo = db.Mensajes.Where(r => r.IdCarpeta == id).Count();

            ViewData["nombrecarpeta"] = proyectos.Nombre + " (" + conteo + ") ";
            ViewBag.IdCarpeta = this.CarpetasCombo();

            var query = db.Mensajes.Include(m => m.Carpetas).AsQueryable();
            query = query.Where(r => r.IdCarpeta == id);
            query = query.Where(r => r.Fecha >= fechainicio);
            query = query.Where(r => r.Fecha <= DateTime.Now);
            query = query.Where(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);
            query = query.OrderByDescending(r => r.Fecha);

            //            this.hola();





            return View(query.ToList());
        }







        [HttpPost]
        public ActionResult Carpetadetalle(int? id, Mensajes support, string start, string end, IList<int> deleteInputs, string submit)
        {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;

            RequerimientosConn db = new RequerimientosConn();

            switch (submit)
            {
                case "Cambiar":

                    if (support.IdCarpeta != null)
                    {

                        if (deleteInputs != null)
                        {
                            var carpe = (from b in db.Carpetas

                                         where b.Id == support.IdCarpeta
                                         select b.Nombre).FirstOrDefault();


                            if (deleteInputs != null || support.IdCarpeta != null)
                            {
                                foreach (var item in deleteInputs)
                                {


                                    try
                                    {


                                        db.AgruparCarpetas(item, support.IdCarpeta);
                                        //support.Id = deleteInputs;
                                        //support.IdCarpeta = support.IdCarpeta;
                                        //db.Entry(support).State = EntityState.Modified;

                                        //db.SaveChanges();

                                    }
                                    catch (Exception ex)
                                    {

                                        log.Error(ex.ToString());
                                        // return RedirectToAction("Index");
                                    }
                                }

                                if (support.IdCarpeta == 1)
                                {
                                    TempData["info"] = "El/los mensaje/s se desvincularon";
                                    log.InfoFormat("Los mensajes se agregaron a {0}", carpe);
                                    return RedirectToAction("Index");
                                }
                                else
                                {

                                    TempData["info"] = "El/los mensaje/s se agregaron a la carpeta " + carpe;
                                    log.InfoFormat("Los mensajes se agregaron a {0}", carpe);
                                    return RedirectToAction("Index");
                                }
                            }
                        }
                        else
                        {
                            TempData["error"] = "No hay mensajes seleccionados";
                            log.Warn("No hay mensajes seleccionados para enviar a la carpeta");
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["error"] = "No hay carpeta de destino seleccionada";
                        log.Warn("No hay carpeta de destino seleccionada para los mensajes");
                        return RedirectToAction("Index");
                    }

                    break;

                default:





                    //foreach (var item in )
                    //{

                    //}
                    //(int i = 0; i < Request.Files.Count; i++)
                    //{
                    //    var file = Request.Files[i];

                    //    if (file != null && file.ContentLength > 0)
                    //    {

                    //    }
                    //}
                    // var hola = deleteInputs;

                    //RequerimientosConn db = new RequerimientosConn();

                    //ViewData["contarmensajeentrada"] =
                    //    db.Mensajes.Include(m => m.Usuarios)
                    //        .Count(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);

                    //ViewData["contarmensajesalida"] =
                    //    db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino != idusuario);

                    //ViewData["Aceptado"] = db.Mensajes.Count(r => r.Status == "Aceptado");
                    //ViewData["Rechazado"] = db.Mensajes.Count(r => r.Status == "Rechazado");
                    //ViewData["Inconsistente"] = db.Mensajes.Count(r => r.Status == "Incosistente");

                    ViewBag.IdCarpeta = this.CarpetasCombo();

                    if (start == "")
                    {
                        TempData["error"] = "Debe seleccionar una fecha";
                        return Index();

                    }
                    else if (end == "")
                    {
                        TempData["error"] = "Debe seleccionar una fecha";
                        return Index();
                    }
                    else if (
                        DateTime.ParseExact(start, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat) >
                        DateTime.ParseExact(end, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat))
                    {
                        TempData["error"] = "Fecha Hasta debe ser mayor";
                        return Index();
                    }
                    else
                    {

                        // var maniana = DateTime.Today.AddDays(-19);
                        DateTime starter = DateTime.ParseExact(start, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                        DateTime ender = DateTime.ParseExact(end, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);


                        //TempData["warning"] = "mensaje de warning!!";
                        //TempData["success"] = "mensaje de success!!";
                        //TempData["info"] = "mensaje de informacion!!";
                        //TempData["error"] = "mensaje de error!!";

                        var query = db.Mensajes.Include(m => m.Carpetas).AsQueryable();
                        query = query.Where(r => r.IdCarpeta == id);
                        query = query.Where(r => r.Fecha >= starter);
                        query = query.Where(r => r.Fecha <= ender);
                        query = query.Where(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);
                        query = query.OrderByDescending(r => r.Fecha);



                        // return View(query.ToList());
                        ViewData["Fechas"] = "Periodo entre: " + start + " y " + end;
                        int count = (from row in db.Mensajes
                                     where row.Fecha >= starter && row.Fecha <= ender && row.IdCarpeta == id
                                     select row).Count();
                        TempData["info"] = "Se encontraron " + count + " registros";

                        return View(query.ToList());

                    }


            }



            return RedirectToAction("Carpetadetalle");








        }










        // [HttpPost]
        public ActionResult Hola(Mensajes support, IList<int> deleteInputs)
        {


            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;

            RequerimientosConn db = new RequerimientosConn();



            if (support.IdCarpeta != null)
            {

                if (deleteInputs != null)
                {



                    if (deleteInputs != null || support.IdCarpeta != null)
                    {
                        foreach (var item in deleteInputs)
                        {

                            try
                            {

                                db.AgruparCarpetas(item, support.IdCarpeta);
                                //support.Id = deleteInputs;
                                //support.IdCarpeta = support.IdCarpeta;
                                //db.Entry(support).State = EntityState.Modified;

                                //db.SaveChanges();

                            }
                            catch (Exception err)
                            {

                                ModelState.AddModelError("", "Failed On Id " + item.ToString() + " : " + err.Message);
                                // return RedirectToAction("Index");
                            }
                        }
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["error"] = "No hay mensajes o carpeta de destino seleccionados";
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }


        #endregion
        //private string SendEmail(string userName, string title, string message)

        //{

        //    string body = string.Empty;
        //    //using streamreader for reading my htmltemplate   

        //    using (StreamReader reader = new StreamReader(Server.MapPath("~/Mail/mailtemplate.html")))

        //    {

        //        body = reader.ReadToEnd();

        //    }

        //    body = body.Replace("{UserName}", userName); //replacing the required things  

        //    body = body.Replace("{Title}", title);

        //    body = body.Replace("{message}", message);

        //    return body;

        //}

        //private void SendHtmlFormattedEmail(string subject, string body)

        //{

        //    using (MailMessage mailMessage = new MailMessage())

        //    {

        //        mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);

        //        mailMessage.Subject = subject;

        //        mailMessage.Body = body;

        //        mailMessage.IsBodyHtml = true;

        //        mailMessage.To.Add(new MailAddress(txt_email.Text));

        //        SmtpClient smtp = new SmtpClient();

        //        smtp.Host = ConfigurationManager.AppSettings["Host"];

        //        smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

        //        System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();

        //        NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"]; //reading from web.config  

        //        NetworkCred.Password = ConfigurationManager.AppSettings["Password"]; //reading from web.config  

        //        smtp.UseDefaultCredentials = true;

        //        smtp.Credentials = NetworkCred;

        //        smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]); //reading from web.config  

        //        smtp.Send(mailMessage);

        //    }

        //}
















        // Envia el mail a la cuenta del usuario seleccionado
        private void SendEmail(string EmailAddress, string txtSubject, string txtMessage, string txtTipoproyecto, string txtRemitente, string txtDestinatario)
        {


            string body = string.Empty;


            var uri = new Uri(Request.Url.AbsoluteUri);
            var url = uri.Scheme + "://" + uri.Host + "/";



            using (StreamReader reader = new StreamReader(Server.MapPath("~/Mail/mailtemplate.html")))

            {

                body = reader.ReadToEnd();

            }
            body = body.Replace("{Url}", url);

            body = body.Replace("{UserName}", EmailAddress); 

            body = body.Replace("{Title}", txtSubject);

            body = body.Replace("{message}", txtMessage);

            body = body.Replace("{txtTipoproyecto}", txtTipoproyecto);

            body = body.Replace("{txtRemitente}", txtRemitente);

            body = body.Replace("{txtDestinatario}", txtDestinatario);



            using (MailMessage mailMessage = new MailMessage())

            {
               

                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);

                mailMessage.Subject = txtSubject;

                mailMessage.Body = body;

                mailMessage.IsBodyHtml = true;

                mailMessage.To.Add(new MailAddress(EmailAddress));

                SmtpClient smtp = new SmtpClient();

                smtp.Host = ConfigurationManager.AppSettings["Host"];

                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();

                NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"]; 

                NetworkCred.Password = ConfigurationManager.AppSettings["Password"];  

                smtp.UseDefaultCredentials = true;

                smtp.Credentials = NetworkCred;

                smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]); 

                smtp.Send(mailMessage);

            }


            //mail.To.Add(EmailAddress);
            //// mail.From = new MailAddress("nicolascuellar85@gmail.com");
            //mail.Subject = txtSubject;
            //mail.Body = txtMessage;


            //mail.IsBodyHtml = true;

            //SmtpClient smtp = new SmtpClient();
            //// smtp.Host = "smtp.gmail.com";
            ////  smtp.Credentials = new System.Net.NetworkCredential("nicolascuellar85@gmail.com", "nicolasvega1212");
            ////  smtp.EnableSsl = true;
            //smtp.Send(mail);

        }




    }
}