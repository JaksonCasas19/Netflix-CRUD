using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieGR8.Models.ViewModel
{
    public class UsuarioVM
    {
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "*Escriba su usuario.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "*Escriba su contraseña.")]
        public string Clave { get; set; }
    }
}
