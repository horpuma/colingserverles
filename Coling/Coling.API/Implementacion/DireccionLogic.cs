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
    public class DireccionLogic : IDireccionLogic
    {
        private readonly Contexto contexto;

        public DireccionLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarDireccion(int id)
        {
            var eliminar = await contexto.Direccions.FindAsync(id);
            if (eliminar == null)
                return false;

            contexto.Direccions.Remove(eliminar);
            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertarDireccion(Direccion insertar)
        {
            bool sw = false;
            contexto.Direccions.Add(insertar);
            int response = await contexto.SaveChangesAsync();
            if (response == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<Direccion>> ListarDireccionTodos()
        {
            var lista = await contexto.Direccions.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarDireccion(Direccion direccion, int id)
        {
            var editar = await contexto.Direccions.FindAsync(id);
            if (editar == null)
                return false;

            editar.IdPersona = direccion.IdPersona;
            editar.Descripcion = direccion.Descripcion;
            editar.Estado = direccion.Estado;

            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<Direccion> ObtenerDireccionById(int id)
        {
            var objeto = await contexto.Direccions.FindAsync(id);
            return objeto;
        }
    }
}
