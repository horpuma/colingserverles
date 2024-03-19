using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColingShared
{
    public interface IExperienciaLaboral
    {
        public int IdAfiliado {  get; set; }
        public string NombreInstitucion {  get; set; }
        public string CargoTitulo {  get; set; }
        public DateTime FechaInicio {  get; set; }
        public DateTime FechaFinal {  get; set; }
        public string Estado {  get; set; }
    }
}
