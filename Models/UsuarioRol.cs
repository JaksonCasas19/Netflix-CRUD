using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieGR8.Models
{
    [Table("UsuarioRol")]
    public partial class UsuarioRol
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }

        [ForeignKey("IdRol")]
        public virtual Roles Rol { get; set; }
    }
}
