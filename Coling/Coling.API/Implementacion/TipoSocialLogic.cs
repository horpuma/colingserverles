using Coling.API.Afiliados.Contratos;
using ColingShared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Implementacion
{
    public class TipoSocialLogic : ITipoSocialLogic
    {
        private readonly Contexto contexto;

        public TipoSocialLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarTipoSocial(int id)
        {
            var eliminar = await contexto.TipoSocials.FindAsync(id);
            if (eliminar == null)
                return false;

            contexto.TipoSocials.Remove(eliminar);
            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertarTipoSocial(TipoSocial insertar)
        {
            bool sw = false;
            contexto.TipoSocials.Add(insertar);
            int response = await contexto.SaveChangesAsync();
            if (response == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<TipoSocial>> ListarTipoSocialTodos()
        {
            var lista = await contexto.TipoSocials.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarTipoSocial(TipoSocial tipoSocial, int id)
        {
            var editar = await contexto.TipoSocials.FindAsync(id);
            if (editar == null)
                return false;

            editar.NombreSocial = tipoSocial.NombreSocial;
            editar.Estado = tipoSocial.Estado;

            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<TipoSocial> ObtenerTipoSocialById(int id)
        {
            var objeto = await contexto.TipoSocials.FindAsync(id);
            return objeto;
        }
    }
}
