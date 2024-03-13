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
    public class TelefonoLogic : ITelefonoLogic
    {
        private readonly Contexto contexto;

        public TelefonoLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarTelefono(int id)
        {
            var telefono = await contexto.Telefonos.FindAsync(id);
            if (telefono == null)
                return false;

            contexto.Telefonos.Remove(telefono);
            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertarTelefono(Telefono telefono)
        {
            bool sw = false;
            contexto.Telefonos.Add(telefono);
            int response = await contexto.SaveChangesAsync();
            if (response == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<Telefono>> ListarTelefonoTodos()
        {
            var lista = await contexto.Telefonos.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarTelefono(Telefono telefono, int id)
        {
            var editar = await contexto.Telefonos.FindAsync(id);
            if (editar == null)
                return false;

            editar.IdPersona = telefono.IdPersona;
            editar.NroTelefono = telefono.NroTelefono;
            editar.Estado = telefono.Estado;

            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<Telefono> ObtenerTelefonoById(int id)
        {
            var telefono = await contexto.Telefonos.FindAsync(id);
            return telefono;
        }
    }
}
