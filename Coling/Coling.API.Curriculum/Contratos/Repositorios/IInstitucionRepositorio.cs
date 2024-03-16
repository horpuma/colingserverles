using Coling.API.Curriculum.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Contratos.Repositorios
{
    public interface IInstitucionRepositorio
    {
        public Task<bool> Create(Institucion institucion);
        public Task<bool> Update(Institucion institucion);
        public Task<bool> Delete(string partitionKey, string rowkey);
        public Task<List<Institucion>> GetAll();
        public Task<Institucion> Get(string id);

    }
}
