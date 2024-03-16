using Coling.API.Curriculum.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Contratos.Repositorios
{
    public interface IExperienciaLaboralRepositorio
    {
        public Task<bool> Create(ExperienciaLaboral experienciaLaboral);
        public Task<bool> Update(ExperienciaLaboral experienciaLaboral);
        public Task<bool> Delete(string partitionKey, string rowkey);
        public Task<List<ExperienciaLaboral>> GetAll();
        public Task<ExperienciaLaboral> Get(string id);
    }
}
