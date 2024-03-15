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
    public class AfiliadoLogic : IAfiliadoLogic
    {
        private readonly Contexto contexto;

        public AfiliadoLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarAfiliado(int id)
        {
            var eliminar = await contexto.Afiliados.FindAsync(id);
            if (eliminar == null)
                return false;

            contexto.Afiliados.Remove(eliminar);
            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertarAfiliado(Afiliado insertar)
        {
            bool sw = false;
            contexto.Afiliados.Add(insertar);
            int response = await contexto.SaveChangesAsync();
            if (response == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<Afiliado>> ListarAfiliadoTodos()
        {
            var lista = await contexto.Afiliados.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarAfiliado(Afiliado afiliado, int id)
        {
            var editar = await contexto.Afiliados.FindAsync(id);
            if (editar == null)
                return false;

            editar.IdPersona = afiliado.IdPersona;
            editar.FechaAfiliacion = afiliado.FechaAfiliacion;
            editar.CodigoAfiliado = afiliado.CodigoAfiliado;
            editar.NroTituloProvisional = afiliado.NroTituloProvisional;
            editar.Estado = afiliado.Estado;

            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<Afiliado> ObtenerAfiliadoById(int id)
        {
            var objeto = await contexto.Afiliados.FindAsync(id);
            return objeto;
        }
    }
}
