using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColingShared
{
    public interface IProfesion
    {
        public string NombreProfesion { get; set; }
        public string NombreGrado { get; set; }
        public string Estado { get; set; }
    }
}
