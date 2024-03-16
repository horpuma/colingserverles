using Azure;
using Azure.Data.Tables;
using ColingShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Modelo
{
    public class ExperienciaLaboral : IExperienciaLaboral, ITableEntity
    {
        public int IdAfiliado { get ; set ; }
        public string NombreInstitucion { get ; set ; }
        public string CargoTitulo { get ; set ; }
        public DateTime FechaInicio { get ; set ; }
        public DateTime FechaFinal { get ; set ; }
        public string Estado { get ; set ; }
        //Table
        public string PartitionKey { get ; set ; }
        public string RowKey { get ; set ; }
        public DateTimeOffset? Timestamp { get ; set ; }
        public ETag ETag { get ; set ; }
    }
}
