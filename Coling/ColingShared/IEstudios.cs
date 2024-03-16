using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColingShared
{
    public interface IEstudios
    {
        public int IdAfiliado {  get; set; }
        public string TipoEstudio {  get; set; }
        public string NombreGrado {  get; set; }
        public string TituloRecibido {  get; set; }
        public string NombreInstitucion {  get; set; }
        public int Anio {  get; set; }
        public string Estado {  get; set; }
    }
}
