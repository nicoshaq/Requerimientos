//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Archivos
    {
        public System.Guid Id { get; set; }
        public string Nombre { get; set; }
        public Nullable<int> Idusuario { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<int> Idmensaje { get; set; }
        public string Extension { get; set; }
        public Nullable<int> Idnovedad { get; set; }
        public Nullable<int> Idproyecto { get; set; }
    
        public virtual Novedades Novedades { get; set; }
        public virtual Proyectos Proyectos { get; set; }
        public virtual Mensajes Mensajes { get; set; }
    }
}
