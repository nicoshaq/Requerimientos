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
using Requerimientos.Controllers;



using System.Collections;
using System.ComponentModel;

using System.Drawing;
using System.Text;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using System.Security.Cryptography.X509Certificates;
using FirmarPDF;

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

        ErrorController error = new ErrorController();


        protected override void Dispose(bool disposing)
        {
            db.Dispose();

           // log.Error(disposing);
            base.Dispose(disposing);
        }

     

        public ActionResult Index()
        {
            log.InfoFormat("{0}", new SUIUsuarios(HttpContext.User.Identity.Name).Usuario);

              var fechainicio = DateTime.Today.AddDays(-365);
           
        idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            ViewData["contarmensajeentrada"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);

            ViewData["contarmensajesalida"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino != idusuario);


            ViewData["Aceptado"] = db.Mensajes.Count(r => r.Status == "Aceptado");
            ViewData["Rechazado"] = db.Mensajes.Count(r => r.Status == "Rechazado");
            ViewData["Inconsistente"] = db.Mensajes.Count(r => r.Status == "Incosistente");

            ViewBag.enviado = 0;

            ViewBag.IdCarpeta = this.CarpetasCombo();

            var query = db.Mensajes.Include(p =>p.Proyectos).AsQueryable();
           
            query = query.Where(r => r.IdCarpeta == 1 || r.IdCarpeta == null);
            query = query.Where(r => r.Fecha >= fechainicio);
            query = query.Where(r => r.Fecha <= DateTime.Now);
            query = query.Where(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);
            query = query.OrderByDescending(r => r.Fecha);


           ViewBag.delegar = idusuario;

            var hola = db.Mensajes.Select(r => r.Idusuariodelega == idusuario && r.Estadodelegado == "Noleido");

            //if (hola == 0)
            //{
            //    ViewData["delegar"] = "delegado";
            //}
            //else
            //{
            //    ViewData["delegar"] = "nodelegado";

            //}



            return View(query.ToList());



        }



      

        [HttpPost]
        public ActionResult Index(Mensajes support, string start, string end, IList<int> deleteInputs, string submit)
        {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;

            RequerimientosConn db = new RequerimientosConn();

            ViewBag.enviado = 0;

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

                        var query = db.Mensajes.Include(m => m.Usuarios).AsQueryable();
                        query = query.Where(r => r.IdCarpeta == 1);
                        query = query.Where(r => r.Fecha >= starter);
                        query = query.Where(r => r.Fecha <= ender);
                        query = query.Where(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);
                        query = query.OrderByDescending(r => r.Fecha);

                        
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

            ViewBag.Idproyecto = this.ProyectoCombo(); 

            ViewBag.User_Id = this.Lista();
            
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
                                        c.EMail,
                                        c.Area.Idarea
                                    };
                    foreach (var c in query_ne1)
                    {
                        support.Idarea = c.Idarea;
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

                 
                    support.IdCarpeta = 1;
                    support.Idproyecto = Idproyecto;
                    support.Fecha = DateTime.Now;
                    support.User_Id = User_Id; //new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
                    support.Remitente = new SUIUsuarios(HttpContext.User.Identity.Name).Usuario;
                    support.Estado = "Noleido";


                    support.Archivos = fileDetails;
                    db.Mensajes.Add(support);
                    db.SaveChanges();

                    //Para enviar mail al destinatario avisando que recibio un nuevo requerimiento
                    //if (support.Enviamail)
                    //{

                    try { 
                        //declaro los datos que se van a enviar al mail

                        string asunto = support.Asunto + " - Requerimientos";
                        string remitente =support.Remitente;
                        string tipoproyecto = proyecto;
                        string mensaje = support.Mensaje;
                        string destinatario = support.Destinatario;

                        string[] to = txtTo.Split(';');
                        txtSubject = asunto;
                        txtTipoproyecto = tipoproyecto;
                        txtRemitente = remitente;
                        txtDestinatario = destinatario;
                        txtMessage =  mensaje;
                        foreach (string emailAdd in to)
                        {

                            if (!string.IsNullOrEmpty(emailAdd))
                                SendEmail(emailAdd, txtSubject, txtMessage, txtTipoproyecto, txtRemitente, txtDestinatario);
                        
                        }
                    }
                    catch (Exception ex) {

                        log.Error(ex.ToString());
                    }

                    //}


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


            if (mensajes == null)
            {
                return error.Error404();
            }

            if (this.HasRole("Administrador"))
            {

                ViewBag.ocultar = false;

            }
            else
            {

                ViewBag.ocultar = true;
            }
            
            if(mensajes.Proyectos.Estado == "Activo")
            {
                ViewBag.estadodos = false;
            }
            else
            {
                ViewBag.estadodos = true;
            }



            if (mensajes.Idusuariodestino != idusuario)
            {
                ViewBag.enviados = true;
            }
            else
            {
                
                    ViewBag.enviados = false;
                
            }



            usuaridelegado = mensajes.Idusuariodelega;

          

            if (mensajes.Idusuariodelega != null) {
                ViewBag.userdelega = new SelectList(db.Usuarios, "User_Id", "Nombre", usuaridelegado);
            }
            else
            {
                ViewBag.userdelega = this.Lista();
            }


            estado = mensajes.Idestado;

            if (mensajes.Idestado !=null) {
                ViewBag.CambioEstado = new SelectList(db.Estado_requerimiento, "Idestado", "Descripcion", estado);

            }else
            {
                ViewBag.CambioEstado = this.CambioEstado();
            }

           
            if (mensajes.Estado == "leido")
            {

                if (mensajes.Estadodelegado == "Noleido" && mensajes.Idusuariodelega == idusuario)
                {
                    mensajes.Estadodelegado = "leido";
                    db.Entry(mensajes).State = EntityState.Modified;
                    db.SaveChanges();

                }

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
        public ActionResult Details(Mensajes support, int? userdelega, int? CambioEstado, string txtTo, string txtSubject, string txtMessage, string txtTipoproyecto, string txtRemitente, string txtDestinatario)
        {

            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;


            var delegacion = (from a in db.Mensajes

                             where a.Id == support.Id
                        select new { a.Idusuariodelega, a.Usuariodelega }).FirstOrDefault();

        

            var estado = (from b in db.Mensajes

                        where b.Id == support.Id
                        select b.Idestado).FirstOrDefault();

          
            try { 


            if (ModelState.IsValid)
            {


                if (userdelega != delegacion.Idusuariodelega) {
                    var query_ne1 = from c in db.Usuarios
                                    where c.User_Id == userdelega
                                    select new
                                    {

                                        c.Nombre,
                                        c.EMail

                                    };
                    foreach (var c in query_ne1)
                    {
                            txtTo = c.EMail;
                            support.Usuariodelega = c.Nombre;
                            support.Estadodelegado = "Noleido";
                        TempData["success"] = "Ha delegado el mensaje a " + c.Nombre;
                        support.Idusuariodelega = userdelega;
                            log.Info("la delegacion de usuario se realizo con exito");
                    }
                 
                    support.User_Id = idusuario;


                        string asunto = "Requerimiento - " + support.Asunto + " - Delegado";
                        string remitente = support.Remitente;
                        string tipoproyecto = proyecto;
                        string mensaje = support.Mensaje;
                        string destinatario = support.Destinatario;

                        string[] to = txtTo.Split(';');
                        txtSubject = asunto; //"alalalalal";
                        txtTipoproyecto = tipoproyecto;
                        txtRemitente = remitente;
                        txtDestinatario = destinatario;
                        txtMessage = mensaje;
                        foreach (string emailAdd in to)
                        {

                            if (!string.IsNullOrEmpty(emailAdd))
                                SendEmail(emailAdd, txtSubject, txtMessage, txtTipoproyecto, txtRemitente, txtDestinatario);

                        }

                        
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


            var fechainicio = DateTime.Today.AddDays(-365);

            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            ViewData["contarmensajeentrada"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);

            ViewData["contarmensajesalida"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino != idusuario);


            ViewData["Aceptado"] = db.Mensajes.Count(r => r.Status == "Aceptado");
            ViewData["Rechazado"] = db.Mensajes.Count(r => r.Status == "Rechazado");
            ViewData["Inconsistente"] = db.Mensajes.Count(r => r.Status == "Incosistente");


            ViewBag.IdCarpeta = this.CarpetasCombo();

            var query = db.Mensajes.Include(p => p.Proyectos).AsQueryable();
            query = query.Where(r => r.Fecha >= fechainicio);
            query = query.Where(r => r.Fecha <= DateTime.Now);
            query = query.Where(r => r.Idusuariodestino != idusuario);
            query = query.OrderByDescending(r => r.Fecha);

            return View(query.ToList());
         }


        [HttpPost]
        public ActionResult Enviados(Mensajes support, string start, string end, string submit)
        {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;

            RequerimientosConn db = new RequerimientosConn();
            
            ViewBag.enviado = 1;

            ViewData["contarmensajeentrada"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);

            ViewData["contarmensajesalida"] = db.Mensajes.Include(m => m.Usuarios).Count(r => r.Idusuariodestino != idusuario);

            ViewData["Aceptado"] = db.Mensajes.Count(r => r.Status == "Aceptado");
                    ViewData["Rechazado"] = db.Mensajes.Count(r => r.Status == "Rechazado");
                    ViewData["Inconsistente"] = db.Mensajes.Count(r => r.Status == "Incosistente");

               

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
                
                        var query = db.Mensajes.Include(m => m.Usuarios).AsQueryable();
                    
                        query = query.Where(r => r.Fecha >= starter);
                        query = query.Where(r => r.Fecha <= ender);
                query = query.Where(r => r.Idusuariodestino != idusuario);
                query = query.OrderByDescending(r => r.Fecha);
                
                        ViewData["Fechas"] = "Periodo entre: " + start + " y " + end;
                        int count = (from row in db.Mensajes
                                     where row.Fecha >= starter && row.Fecha <= ender && row.Idusuariodestino !=idusuario
                                     select row).Count();
                        TempData["info"] = "Se encontraron " + count + " registros";

                        return View(query.ToList());

                    }

        }






       
        public FileResult Descarga(String p, String d)
        {
            return File(Path.Combine(Server.MapPath("~/Content/Upload/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }
        
        /// <summary>
        /// List all of the form fields into a textbox.  The
        /// form fields identified can be used to map each of the
        /// fields in a PDF.
        /// </summary>
        private void ListFieldNames()
        {
            string pdfTemplate = @"C:\log\prueba.pdf";
           
            PdfReader pdfReader = new PdfReader(pdfTemplate);
         
            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry de in pdfReader.AcroFields.Fields)
            {
                sb.Append(de.Key.ToString() + Environment.NewLine);
            }
          
        }

        
        public virtual ActionResult FillForm(int? id, bool firma = false)
        {
            
            Mensajes support = db.Mensajes.Include(u => u.Usuarios).SingleOrDefault(x => x.Id == id);

            string pdfTemplate = Server.MapPath("~/pdf/base/" + "requerimiento.pdf");
            
            string newFile = Server.MapPath("~/pdf/" + support.Id+ "_"+ support.Asunto +".pdf");
            
            PdfReader pdfReader = new PdfReader(pdfTemplate);

            PdfReader pdfReader2 = new PdfReader(pdfTemplate);

            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
            AcroFields pdfFormFields = pdfStamper.AcroFields;


            var outputpdfStream = new MemoryStream();

            var stampler = new PdfStamper(pdfReader2, outputpdfStream)
            {
                FormFlattening = false,
                FreeTextFlattening = false
            };
            
            pdfFormFields.SetField("Id", (support.Id).ToString());
            pdfFormFields.SetField("Asunto", support.Asunto);
            pdfFormFields.SetField("Areasolicitante", support.Asunto);
            pdfFormFields.SetField("Usuariosolicitante", support.Usuarios.Nombre);
            pdfFormFields.SetField("Prioridad", support.Prioridad);
            pdfFormFields.SetField("Naturaleza", support.Naturaleza);
            pdfFormFields.SetField("Descripcion", support.Descripcion);
            pdfFormFields.SetField("Objetivo", support.Objetivo);
            pdfFormFields.SetField("Alcance", support.Alcance);
           

            pdfFormFields.SetField("Func1", support.Func1);
            pdfFormFields.SetField("Func2", support.Func2);
            pdfFormFields.SetField("Func3", support.Func3);
            pdfFormFields.SetField("Func4", support.Func4);
            pdfFormFields.SetField("Func5", support.Func5);
            pdfFormFields.SetField("Func6", support.Func6);

            pdfFormFields.SetField("Impresa1", support.Impresa1);
            pdfFormFields.SetField("Impresa2", support.Impresa2);
            pdfFormFields.SetField("Impresa3", support.Impresa3);

            pdfFormFields.SetField("Pantalla1", support.Pantalla1);
            pdfFormFields.SetField("Pantalla2", support.Pantalla2);
            pdfFormFields.SetField("Pantalla3", support.Pantalla3);


            pdfFormFields.SetField("Porarchivo1", support.Porarchivo1);
            pdfFormFields.SetField("Porarchivo2", support.Porarchivo2);
            pdfFormFields.SetField("Porarchivo3", support.Porarchivo3);



            pdfFormFields.SetField("Nueva1", (support.Nueva1).ToString());
            pdfFormFields.SetField("Nueva2", (support.Nueva2).ToString());
            pdfFormFields.SetField("Nueva3", (support.Nueva3).ToString());
            pdfFormFields.SetField("Nueva4", (support.Nueva4).ToString());
            pdfFormFields.SetField("Nueva5", (support.Nueva5).ToString());
            pdfFormFields.SetField("Nueva6", (support.Nueva6).ToString());


            pdfFormFields.SetField("Modif1", (support.Modif1).ToString());
            pdfFormFields.SetField("Modif2", (support.Modif2).ToString());
            pdfFormFields.SetField("Modif3", (support.Modif3).ToString());
            pdfFormFields.SetField("Modif4", (support.Modif4).ToString());
            pdfFormFields.SetField("Modif5", (support.Modif5).ToString());
            pdfFormFields.SetField("Modif6", (support.Modif6).ToString());

            pdfFormFields.SetField("Eliminar1", (support.Eliminar1).ToString());
            pdfFormFields.SetField("Eliminar2", (support.Eliminar2).ToString());
            pdfFormFields.SetField("Eliminar3", (support.Eliminar3).ToString());
            pdfFormFields.SetField("Eliminar4", (support.Eliminar4).ToString());
            pdfFormFields.SetField("Eliminar5", (support.Eliminar5).ToString());
            pdfFormFields.SetField("Eliminar6", (support.Eliminar6).ToString());


            pdfFormFields.SetField("Nuevaimp1", (support.Nuevaimp1).ToString());
            pdfFormFields.SetField("Nuevaimp2", (support.Nuevaimp2).ToString());
            pdfFormFields.SetField("Nuevaimp3", (support.Nuevaimp3).ToString());

            pdfFormFields.SetField("Modifimp1", (support.Modifimp1).ToString());
            pdfFormFields.SetField("Modifimp2", (support.Modifimp2).ToString());
            pdfFormFields.SetField("Modifimp3", (support.Modifimp3).ToString());

            pdfFormFields.SetField("Eliminaimp1", (support.Eliminaimp1).ToString());
            pdfFormFields.SetField("Eliminaimp2", (support.Eliminaimp2).ToString());
            pdfFormFields.SetField("Eliminaimp3", (support.Eliminaimp3).ToString());




            pdfFormFields.SetField("Nuevapant1", (support.Nuevapant1).ToString());
            pdfFormFields.SetField("Nuevapant2", (support.Nuevapant2).ToString());
            pdfFormFields.SetField("Nuevapant3", (support.Nuevapant3).ToString());

            pdfFormFields.SetField("Modifpant1", (support.Modifpant1).ToString());
            pdfFormFields.SetField("Modifpant2", (support.Modifpant2).ToString());
            pdfFormFields.SetField("Modifpant3", (support.Modifpant3).ToString());

            pdfFormFields.SetField("Eliminapant1", (support.Eliminapant1).ToString());
            pdfFormFields.SetField("Eliminapant2", (support.Eliminapant2).ToString());
            pdfFormFields.SetField("Eliminapant3", (support.Eliminapant3).ToString());




            pdfFormFields.SetField("Nuevarch1", (support.Nuevarch1).ToString());
            pdfFormFields.SetField("Nuevarch2", (support.Nuevarch2).ToString());
            pdfFormFields.SetField("Nuevarch3", (support.Nuevarch3).ToString());

            pdfFormFields.SetField("Modifarch1", (support.Modifarch1).ToString());
            pdfFormFields.SetField("Modifarch2", (support.Modifarch2).ToString());
            pdfFormFields.SetField("Modifarch3", (support.Modifarch3).ToString());

            pdfFormFields.SetField("Eliminarch1", (support.Eliminarch1).ToString());
            pdfFormFields.SetField("Eliminarch2", (support.Eliminarch2).ToString());
            pdfFormFields.SetField("Eliminarch3", (support.Eliminarch3).ToString());
                        
            pdfFormFields.SetField("Ventaja", support.Ventaja);
            pdfFormFields.SetField("Arearelacion", support.Arearelacion);
            pdfFormFields.SetField("Afectado", support.Afectado);
            pdfFormFields.SetField("Normas", support.Normas);

            pdfFormFields.SetField("Area1", support.Area1);
            pdfFormFields.SetField("Area2", support.Area2);
            pdfFormFields.SetField("Area3", support.Area3);

            pdfFormFields.SetField("Gerente1", support.Gerente1);
            pdfFormFields.SetField("Gerente2", support.Gerente2);
            pdfFormFields.SetField("Gerente3", support.Gerente3);

            pdfFormFields.SetField("Firma1", support.Firma1);
            pdfFormFields.SetField("Firma2", support.Firma2);
            pdfFormFields.SetField("Firma3", support.Firma3);

            pdfFormFields.SetField("Confeccionado", support.Usuarios.Nombre);
            pdfFormFields.SetField("Recepcionado", support.Recepcionado);

            pdfStamper.FormFlattening = false;
            pdfStamper.FreeTextFlattening = false;
           
            pdfStamper.Close();
            
            if (firma == false)
            {

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + support.Asunto + ".pdf");
                Response.TransmitFile(newFile);
                Response.End();

            }

            support.Status = "check";

            db.Entry(support).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Details", "Mensajes", new { id = id });
          
        }


        public virtual ActionResult Pdfdowload(int? id, HttpPostedFileBase photo)
        {

            string pdfTemplate = Server.MapPath("~/pdf/base/" + "requerimiento.pdf");
            

            PdfReader pdfReader = new PdfReader(pdfTemplate);

            var outputpdfStream = new MemoryStream();

            var stampler = new PdfStamper(pdfReader, outputpdfStream)
            {
                FormFlattening = false,
                FreeTextFlattening = false
            };

            AcroFields pdfFormFields = stampler.AcroFields;

            Mensajes support = db.Mensajes.Include(u => u.Usuarios).SingleOrDefault(x => x.Id == id);

            pdfFormFields.SetField("Id",  (support.Id).ToString());
            pdfFormFields.SetField("Asunto", support.Asunto);
            pdfFormFields.SetField("Areasolicitante", support.Asunto);
            pdfFormFields.SetField("Usuariosolicitante", support.Usuarios.Nombre);
            pdfFormFields.SetField("Prioridad", support.Prioridad);
            pdfFormFields.SetField("Naturaleza", support.Naturaleza);
            pdfFormFields.SetField("Descripcion", support.Descripcion);
            pdfFormFields.SetField("Objetivo", support.Objetivo);
            pdfFormFields.SetField("Alcance", support.Alcance);
           

            pdfFormFields.SetField("Func1", support.Func1);
            pdfFormFields.SetField("Func2", support.Func2);
            pdfFormFields.SetField("Func3", support.Func3);
            pdfFormFields.SetField("Func4", support.Func4);
            pdfFormFields.SetField("Func5", support.Func5);
            pdfFormFields.SetField("Func6", support.Func6);

            pdfFormFields.SetField("Impresa1", support.Impresa1);
            pdfFormFields.SetField("Impresa2", support.Impresa2);
            pdfFormFields.SetField("Impresa3", support.Impresa3);

            pdfFormFields.SetField("Pantalla1", support.Pantalla1);
            pdfFormFields.SetField("Pantalla2", support.Pantalla2);
            pdfFormFields.SetField("Pantalla3", support.Pantalla3);


            pdfFormFields.SetField("Porarchivo1", support.Porarchivo1);
            pdfFormFields.SetField("Porarchivo2", support.Porarchivo2);
            pdfFormFields.SetField("Porarchivo3", support.Porarchivo3);



            pdfFormFields.SetField("Nueva1", (support.Nueva1).ToString());
            pdfFormFields.SetField("Nueva2", (support.Nueva2).ToString());
            pdfFormFields.SetField("Nueva3", (support.Nueva3).ToString());
            pdfFormFields.SetField("Nueva4", (support.Nueva4).ToString());
            pdfFormFields.SetField("Nueva5", (support.Nueva5).ToString());
            pdfFormFields.SetField("Nueva6", (support.Nueva6).ToString());


            pdfFormFields.SetField("Modif1", (support.Modif1).ToString());
            pdfFormFields.SetField("Modif2", (support.Modif2).ToString());
            pdfFormFields.SetField("Modif3", (support.Modif3).ToString());
            pdfFormFields.SetField("Modif4", (support.Modif4).ToString());
            pdfFormFields.SetField("Modif5", (support.Modif5).ToString());
            pdfFormFields.SetField("Modif6", (support.Modif6).ToString());

            pdfFormFields.SetField("Eliminar1", (support.Eliminar1).ToString());
            pdfFormFields.SetField("Eliminar2", (support.Eliminar2).ToString());
            pdfFormFields.SetField("Eliminar3", (support.Eliminar3).ToString());
            pdfFormFields.SetField("Eliminar4", (support.Eliminar4).ToString());
            pdfFormFields.SetField("Eliminar5", (support.Eliminar5).ToString());
            pdfFormFields.SetField("Eliminar6", (support.Eliminar6).ToString());


            pdfFormFields.SetField("Nuevaimp1", (support.Nuevaimp1).ToString());
            pdfFormFields.SetField("Nuevaimp2", (support.Nuevaimp2).ToString());
            pdfFormFields.SetField("Nuevaimp3", (support.Nuevaimp3).ToString());

            pdfFormFields.SetField("Modifimp1", (support.Modifimp1).ToString());
            pdfFormFields.SetField("Modifimp2", (support.Modifimp2).ToString());
            pdfFormFields.SetField("Modifimp3", (support.Modifimp3).ToString());

            pdfFormFields.SetField("Eliminaimp1", (support.Eliminaimp1).ToString());
            pdfFormFields.SetField("Eliminaimp2", (support.Eliminaimp2).ToString());
            pdfFormFields.SetField("Eliminaimp3", (support.Eliminaimp3).ToString());




            pdfFormFields.SetField("Nuevapant1", (support.Nuevapant1).ToString());
            pdfFormFields.SetField("Nuevapant2", (support.Nuevapant2).ToString());
            pdfFormFields.SetField("Nuevapant3", (support.Nuevapant3).ToString());

            pdfFormFields.SetField("Modifpant1", (support.Modifpant1).ToString());
            pdfFormFields.SetField("Modifpant2", (support.Modifpant2).ToString());
            pdfFormFields.SetField("Modifpant3", (support.Modifpant3).ToString());

            pdfFormFields.SetField("Eliminapant1", (support.Eliminapant1).ToString());
            pdfFormFields.SetField("Eliminapant2", (support.Eliminapant2).ToString());
            pdfFormFields.SetField("Eliminapant3", (support.Eliminapant3).ToString());




            pdfFormFields.SetField("Nuevarch1", (support.Nuevarch1).ToString());
            pdfFormFields.SetField("Nuevarch2", (support.Nuevarch2).ToString());
            pdfFormFields.SetField("Nuevarch3", (support.Nuevarch3).ToString());

            pdfFormFields.SetField("Modifarch1", (support.Modifarch1).ToString());
            pdfFormFields.SetField("Modifarch2", (support.Modifarch2).ToString());
            pdfFormFields.SetField("Modifarch3", (support.Modifarch3).ToString());

            pdfFormFields.SetField("Eliminarch1", (support.Eliminarch1).ToString());
            pdfFormFields.SetField("Eliminarch2", (support.Eliminarch2).ToString());
            pdfFormFields.SetField("Eliminarch3", (support.Eliminarch3).ToString());





            pdfFormFields.SetField("Ventaja", support.Ventaja);
            pdfFormFields.SetField("Arearelacion", support.Arearelacion);
            pdfFormFields.SetField("Afectado", support.Afectado);
            pdfFormFields.SetField("Normas", support.Normas);

            pdfFormFields.SetField("Area1", support.Area1);
            pdfFormFields.SetField("Area2", support.Area2);
            pdfFormFields.SetField("Area3", support.Area3);

            pdfFormFields.SetField("Gerente1", support.Gerente1);
            pdfFormFields.SetField("Gerente2", support.Gerente2);
            pdfFormFields.SetField("Gerente3", support.Gerente3);

            pdfFormFields.SetField("Firma1", support.Firma1);
            pdfFormFields.SetField("Firma2", support.Firma2);
            pdfFormFields.SetField("Firma3", support.Firma3);

            pdfFormFields.SetField("Confeccionado", support.Usuarios.Nombre);
            pdfFormFields.SetField("Recepcionado", support.Recepcionado);

            stampler.SetFullCompression();

            stampler.Close();

          
            var file = outputpdfStream.ToArray();
            var output = new MemoryStream();
            output.Write(file, 0, file.Length);
            output.Position = 0;

            var cd = new System.Net.Mime.ContentDisposition
            {

                FileName = support.Asunto +"_"+ DateTime.Today + ".pdf",
                Inline = false,
                Size = file.Length,
                CreationDate = DateTime.Now

            };
            
            //Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(output, System.Net.Mime.MediaTypeNames.Application.Pdf);



        }

        
        public List<SelectListItem> CambioEstado()
        {

            var content = from p in db.Estado_requerimiento
                         
                          select new { p.Idestado, p.Descripcion };

            ViewBag.estado = content
                .Select(c => new SelectListItem
                {
                    Text = c.Descripcion,
                    Value = c.Idestado.ToString()
                }).ToList();

            return ViewBag.estado;
          
        }



        public List<SelectListItem> Lista()
        {
            var content = from p in db.Usuarios
                          where p.Inactivo == false || p.User_Id == usuaridelegado
                          select new { p.User_Id, p.Nombre };

            ViewBag.usuario = content
                .Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.User_Id.ToString()
                }).ToList();

            return ViewBag.usuario;
        }



        public List<SelectListItem> Listadelegado(int? usuaridelegado)
        {
            idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id;
            var content = from p in db.Usuarios
                          where p.Inactivo == false || p.User_Id != idusuario
                          select new { p.User_Id, p.Nombre };

            ViewBag.usuario = content
               .Where(c => c.User_Id != usuaridelegado)
                .Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.User_Id.ToString()
                })
                .ToList();

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
                                     
                                    }
                                    catch (Exception ex)
                                    {

                                        log.Error(ex.ToString());
                                      
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

                        
                        DateTime starter = DateTime.ParseExact(start, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                        DateTime ender = DateTime.ParseExact(end, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);

                        var query = db.Mensajes.Include(m => m.Carpetas).AsQueryable();
                        query = query.Where(r => r.IdCarpeta == id);
                        query = query.Where(r => r.Fecha >= starter);
                        query = query.Where(r => r.Fecha <= ender);
                        query = query.Where(r => r.Idusuariodestino == idusuario || r.Idusuariodelega == idusuario);
                        query = query.OrderByDescending(r => r.Fecha);



                       
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
                              
                            }
                            catch (Exception err)
                            {

                                ModelState.AddModelError("", "Failed On Id " + item.ToString() + " : " + err.Message);
                              
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

                //smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]); 

                smtp.Send(mailMessage);

            }



        }










        public virtual ActionResult Firmar(int? id)
        {

            FillForm(id, true);



            Mensajes support = db.Mensajes.Include(u => u.Usuarios).SingleOrDefault(x => x.Id == id);

            if(support.Firma1 == null)
            {

                try
                {

                    string pdfTemplate = Server.MapPath("~/pdf/" + support.Id + "_" + support.Asunto + ".pdf");


                    X509Store objStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                    objStore.Open(OpenFlags.ReadOnly);
                    X509Certificate2 objCert = null;
                    if (objStore.Certificates != null)
                        foreach (X509Certificate2 objCertTemp in objStore.Certificates)
                            if (objCertTemp.HasPrivateKey)
                            {
                                objCert = objCertTemp;
                                break;
                            }

                    string hola = "algosss";

                    List<Archivos> fileDetails = new List<Archivos>();



                    Archivos fileDetail = new Archivos()
                    {

                        Nombre = DateTime.Now + "_" + support.Asunto + ".pdf",
                        Extension = ".pdf",
                        Id = Guid.NewGuid(),
                        Fecha = DateTime.Now,
                        Idusuario = new SUIUsuarios(HttpContext.User.Identity.Name).User_Id,
                        Idproyecto = support.Proyectos.Idproyecto,
                        Idmensaje = support.Id,



                    };
                    fileDetails.Add(fileDetail);

                    string pdfTemplatesalida = Path.Combine(Server.MapPath("~/Content/Upload/"), fileDetail.Id + fileDetail.Extension);

                    PDF.SignHashed(
                   pdfTemplate,
                   pdfTemplatesalida,
                   objCert,
                   "Autorizacion " + support.Asunto,
                   "Argentina",
                   true);


                    if (System.IO.File.Exists(Server.MapPath("~/pdf/" + support.Id + "_" + support.Asunto + ".pdf")))
                    {
                        try
                        {
                            System.IO.File.Delete(Server.MapPath("~/pdf/" + support.Id + "_" + support.Asunto + ".pdf"));
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine(e.Message);

                        }

                    }


                    db.Entry(fileDetail).State = EntityState.Added;

                    db.SaveChanges();

                    support.Firma1 = objCert.Subject;

                    db.Entry(support).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = id });

                }
                catch (Exception ex)
                {
                    log.Error(ex.ToString());
                }

            }
            else
            {

                log.Info("firma completasssss");
            }
            
            return RedirectToAction("Details", new { id = id });
        }

    }
}