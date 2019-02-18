using Requerimientos.Models;

namespace RBACModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RBAC_Model : DbContext
    {
        public RBAC_Model()
            : base("name=RBAC_Model")
        {
        }

        public virtual DbSet<Permisos> PERMISOS { get; set; }
        public virtual DbSet<ROLES> ROLES { get; set; }
        public virtual DbSet<Usuarios> USERS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permisos>()
                .HasMany(e => e.ROLES)
                .WithMany(e => e.Permisos)
                .Map(m => m.ToTable("LNK_ROL_Permiso").MapLeftKey("Permiso_Id").MapRightKey("Rol_Id"));

            modelBuilder.Entity<ROLES>()
                .HasMany(e => e.Usuarios)
                .WithMany(e => e.ROLES)
                .Map(m => m.ToTable("LNK_Usuario_ROL").MapLeftKey("Rol_Id").MapRightKey("User_Id"));
        }
    }
}
