using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieGR8.Models
{
    [Table("Usuarios")]
    public partial class Usuarios
    {
        [Key]
        public int IdUsuario { get; set; }
        [Required(ErrorMessage="*Escriba el nombre del usuario.")]
        [MinLength(5,ErrorMessage ="Escriba al menos 5 caracteres")]
        [MaxLength(50,ErrorMessage ="Escriba un máximo de 50 caracteres")]
        public string Nombre { get; set; }
        public string Sal { get; set; }
        [Required(ErrorMessage = "*Escriba la contraseña.")]
        [MinLength(5, ErrorMessage = "Escriba al menos 5 caracteres")]
        [MaxLength(50, ErrorMessage = "Escriba un máximo de 50 caracteres")]
        public string Clave { get; set; }
        public int Edad { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual List<UsuarioRol> Roles { get; set; }

    } 
}
