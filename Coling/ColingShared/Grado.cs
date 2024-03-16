using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColingShared
{
    public class Grado
    {
        [Key]
        public int Id { get; set; }
        public string? NombreGrado { get; set; }
        public string? Estado { get; set; }
    }
}
