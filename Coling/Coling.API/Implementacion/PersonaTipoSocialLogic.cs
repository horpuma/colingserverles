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
    public class PersonaTipoSocialLogic : IPersonaTipoSocialLogic
    {
        private readonly Contexto contexto;

        public PersonaTipoSocialLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarPersonaTipoSocial(int id)
        {
            var eliminar = await contexto.PersonaTipoSocials.FindAsync(id);
            if (eliminar == null)
                return false;

            contexto.PersonaTipoSocials.Remove(eliminar);
            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertarPersonaTipoSocial(PersonaTipoSocial insertar)
        {
            bool sw = false;
            contexto.PersonaTipoSocials.Add(insertar);
            int response = await contexto.SaveChangesAsync();
            if (response == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<PersonaTipoSocial>> ListarPersonaTipoSocialTodos()
        {
            var lista = await contexto.PersonaTipoSocials.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarPersonaTipoSocial(PersonaTipoSocial personaTipoSocial, int id)
        {
            var editar = await contexto.PersonaTipoSocials.FindAsync(id);
            if (editar == null)
                return false;

            editar.IdTipoSocial = personaTipoSocial.IdTipoSocial;
            editar.IdPersona = personaTipoSocial.IdPersona;
            editar.Estado = personaTipoSocial.Estado;

            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<PersonaTipoSocial> ObtenerPersonaTipoSocialById(int id)
        {
            var objeto = await contexto.PersonaTipoSocials.FindAsync(id);
            return objeto;
        }
    }
}
