using Coling.Repositorio.Implementacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Repositorio.Contratos
{
    public interface IUsuarioRepositorio
    {
        public Task<string> EncriptarPassword(string password);
        public Task<TokenData> VerificarCredenciales(string usuariox, string passwordx);
        public Task<bool> ValidarToken(string token);
        public Task<TokenData> ConstruirToken(string username, string password);
    }
}
