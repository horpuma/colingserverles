using ColingShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.BolsaTrabajo.Contratos
{
    public interface IOfertaLaboralLogic
    {
        Task<bool> InsertarOfertaLaboral(OfertaLaboral ofertaLaboral);
        Task<bool> ModificarOfertaLaboral(OfertaLaboral ofertaLaboral, string id);
        Task<bool> EliminarOfertaLaboral(string id);
        Task<List<OfertaLaboral>> ListarOfertasLaborales();
        Task<OfertaLaboral> ObtenerOfertaLaboralById(string id);
    }
}
