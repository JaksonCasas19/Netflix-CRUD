using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieGR8.Models
{
    [Table("Roles")]
    public partial class Roles
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None),Column("Id")]
        public int IdRol { get; set; }

        public string Descripcion { get; set; }
    }
}
