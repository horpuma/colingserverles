using Coling.API.Afiliados.Contratos;
using ColingShared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Coling.API.Afiliados.Implementacion
{
    public class PersonaLogic : IPersonaLogic
    {
        private readonly Contexto contexto;

        public PersonaLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> EliminarPersona(int id)
        {
            var persona = await contexto.Personas.FindAsync(id);
            if (persona == null)
                return false;

            contexto.Personas.Remove(persona);
            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertarPersona(Persona persona)
        {
            bool sw = false;
            contexto.Personas.Add(persona);
            int response = await contexto.SaveChangesAsync();
            if (response == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<Persona>> ListarPersonaTodos()
        {
            var lista = await contexto.Personas.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarPersona(Persona persona, int id)
        {
            var editar = await contexto.Personas.FindAsync(id);
            if (editar == null)
                return false;

            editar.Nombre = persona.Nombre;
            editar.Apellidos = persona.Apellidos;
            editar.FechaNacimiento = persona.FechaNacimiento;
            editar.Foto = persona.Foto;
            editar.Estago = persona.Estago;

            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<Persona> ObtenerPersonaById(int id)
        {
            var persona = await contexto.Personas.FindAsync(id);
            return persona;
        }
    }
}
