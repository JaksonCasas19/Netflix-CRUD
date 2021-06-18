using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieGR8.Models
{
    public class LoginContext:DbContext
    {
        public LoginContext(DbContextOptions<LoginContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builer)
        {
            builer.Entity<UsuarioRol>().HasKey(x => new { x.IdUsuario, x.IdRol });
        }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Roles> UsuarioRol { get; set; }
        public DbSet<PeliculaModel> Peliculas { get; set; }
    }
}
