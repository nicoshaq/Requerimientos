﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Requerimientos.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class RequerimientosConn : DbContext
    {
        public RequerimientosConn()
            : base("name=RequerimientosConn")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Archivos> Archivos { get; set; }
        public virtual DbSet<Mensajes> Mensajes { get; set; }
        public virtual DbSet<Permisos> Permisos { get; set; }
        public virtual DbSet<ROLES> ROLES { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Novedades> Novedades { get; set; }
        public virtual DbSet<Proyectos> Proyectos { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Estado_novedades> Estado_novedades { get; set; }
        public virtual DbSet<Estado_requerimiento> Estado_requerimiento { get; set; }
        public virtual DbSet<HistorialDelega> HistorialDelega { get; set; }
        public virtual DbSet<Carpetas> Carpetas { get; set; }
    
        public virtual int updateidlote()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("updateidlote");
        }
    
        public virtual int updateidloteReportes()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("updateidloteReportes");
        }
    
        public virtual ObjectResult<Mensajeria_Result> Mensajeria()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Mensajeria_Result>("Mensajeria");
        }
    
        public virtual ObjectResult<Mensajes> Mensajerias()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Mensajes>("Mensajerias");
        }
    
        public virtual ObjectResult<Mensajes> Mensajerias(MergeOption mergeOption)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Mensajes>("Mensajerias", mergeOption);
        }
    
        public virtual int historialDelegacion(Nullable<int> idmensaje, Nullable<int> idproyecto, string usuariodelega, string usuariodelegado)
        {
            var idmensajeParameter = idmensaje.HasValue ?
                new ObjectParameter("Idmensaje", idmensaje) :
                new ObjectParameter("Idmensaje", typeof(int));
    
            var idproyectoParameter = idproyecto.HasValue ?
                new ObjectParameter("Idproyecto", idproyecto) :
                new ObjectParameter("Idproyecto", typeof(int));
    
            var usuariodelegaParameter = usuariodelega != null ?
                new ObjectParameter("Usuariodelega", usuariodelega) :
                new ObjectParameter("Usuariodelega", typeof(string));
    
            var usuariodelegadoParameter = usuariodelegado != null ?
                new ObjectParameter("Usuariodelegado", usuariodelegado) :
                new ObjectParameter("Usuariodelegado", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("historialDelegacion", idmensajeParameter, idproyectoParameter, usuariodelegaParameter, usuariodelegadoParameter);
        }
    
        public virtual int AgruparCarpetas(Nullable<int> idmensaje, Nullable<int> idcarpeta)
        {
            var idmensajeParameter = idmensaje.HasValue ?
                new ObjectParameter("Idmensaje", idmensaje) :
                new ObjectParameter("Idmensaje", typeof(int));
    
            var idcarpetaParameter = idcarpeta.HasValue ?
                new ObjectParameter("Idcarpeta", idcarpeta) :
                new ObjectParameter("Idcarpeta", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AgruparCarpetas", idmensajeParameter, idcarpetaParameter);
        }
    }
}
