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
    public class ProfesionAfiliadoLogic : IProfesionAfiliadoLogic
    {
        private readonly Contexto contexto;

        public ProfesionAfiliadoLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarProfesionAfiliado(int id)
        {
            var eliminar = await contexto.ProfesionAfiliados.FindAsync(id);
            if (eliminar == null)
                return false;

            contexto.ProfesionAfiliados.Remove(eliminar);
            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertarProfesionAfiliado(ProfesionAfiliado insertar)
        {
            bool sw = false;
            contexto.ProfesionAfiliados.Add(insertar);
            int response = await contexto.SaveChangesAsync();
            if (response == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<ProfesionAfiliado>> ListarProfesionAfiliadoTodos()
        {
            var lista = await contexto.ProfesionAfiliados.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarProfesionAfiliado(ProfesionAfiliado profesionAfiliado, int id)
        {
            var editar = await contexto.ProfesionAfiliados.FindAsync(id);
            if (editar == null)
                return false;

            editar.IdAfiliado = profesionAfiliado.IdAfiliado;
            editar.IdProfesion = profesionAfiliado.IdProfesion;
            editar.FechaAsignacion = profesionAfiliado.FechaAsignacion;
            editar.Nrosellosib = profesionAfiliado.Nrosellosib;
            editar.Estado = profesionAfiliado.Estado;

            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<ProfesionAfiliado> ObtenerProfesionAfiliadoById(int id)
        {
            var objeto = await contexto.ProfesionAfiliados.FindAsync(id);
            return objeto;
        }
    }
}
