using Coling.API.Curriculum.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Contratos.Repositorios
{
    public interface IProfesionRepositorio
    {
        public Task<bool> Create(Profesion profesion);
        public Task<bool> Update(Profesion profesion);
        public Task<bool> Delete(string partitionKey, string rowkey);
        public Task<List<Profesion>> GetAll();
        public Task<Profesion> Get(string id);
    }
}
