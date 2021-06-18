using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieGR8.Models
{
    public class PeliculaModel
    {
        [Key]
        public int IdPelicula { get; set; }
        public string Titulo { get; set; }
        public string YearMovie { get; set; }
        public string Director { get; set; }
        public string Descripcion { get; set; }
        public string Company { get; set; }
        public string Categoria { get; set; }
        public string Thumbnail { get; set; }
        public string TrailerVideo { get; set; }
        public string CreatedBy { get; set; }

    }
}
