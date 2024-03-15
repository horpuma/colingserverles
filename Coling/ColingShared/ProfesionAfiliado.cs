using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColingShared
{
    public class ProfesionAfiliado
    {
        [Key]
        public int Id { get;set; }
        public string IdProfesion {  get; set; }   
        public int IdAfiliado { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public string? Nrosellosib {  get; set; }
        public string? Estado {  get; set; }
        [ForeignKey("IdAfiliado")]
        public virtual Afiliado? IdAfiliadoNavigation { get; set; }
        //[ForeignKey("IdProfesion")]
        //public virtual Profesion? IdProfesionNavigation { get; set; }

    }
}
