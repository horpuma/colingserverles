using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ColingShared
{
    public class Telefono
    {
        [Key]
        public int Id { get; set; }
        public int IdPersona { get; set; }
        public string? NroTelefono { get; set; }
        public string? Estado {  get; set; }

        [ForeignKey("IdPersona")]
        /*[InverseProperty("Telefonos")]*/
        public virtual Persona? IdPersonaNavigation { get; set; }

    }
}
