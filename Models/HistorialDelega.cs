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
    
    public partial class HistorialDelega
    {
        public int Idhistorial { get; set; }
        public Nullable<int> Idproyecto { get; set; }
        public Nullable<int> Idmensaje { get; set; }
        public string Usuariodelega { get; set; }
        public string Usuariodelegado { get; set; }
    
        public virtual Mensajes Mensajes { get; set; }
        public virtual Proyectos Proyectos { get; set; }
    }
}
