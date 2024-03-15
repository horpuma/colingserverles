using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColingShared
{
    public class Persona
    {
        [Key]
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Foto { get; set; }
        public string? Estago { get; set; }

        /*[InverseProperty("IdPersonaNavigation")]
        public virtual ICollection<Telefono>? Telefonos { get; set; } = new List<Telefono>();*/
    }
}
