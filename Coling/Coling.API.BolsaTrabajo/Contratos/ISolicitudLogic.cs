using ColingShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.BolsaTrabajo.Contratos
{
    public interface ISolicitudLogic
    {
        Task<bool> InsertarSolicitud(Solicitud solicitud);
        Task<bool> ModificarSolicitud(Solicitud solicitud, string id);
        Task<bool> EliminarSolicitud(string id);
        Task<List<Solicitud>> ListarSolicitudes();
        Task<Solicitud> ObtenerSolicitudById(string id);
    }
}
