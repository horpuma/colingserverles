using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColingShared
{
    public class Direccion
    {
        [Key]
        public int Id { get; set; }
        public int IdPersona { get; set; }
        public string? Descripcion { get; set;}
        public string? Estado { get; set; }
        [ForeignKey("IdPersona")]
        public virtual Persona? IdPersonaNavigation { get; set; }
    }
}
