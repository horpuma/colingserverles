using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColingShared
{
    public class Profesion
    {
        [Key]
        public int Id { get; set; }
        public string? NombreProfesion { get; set; }
        public int IdGrado { get; set; }
        public string? Estado { get; set; }
        [ForeignKey("IdGrado")]
        public virtual Grado? IdGradoNavigation { get; set; }
    }
}
