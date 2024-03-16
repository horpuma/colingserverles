using Azure.Data.Tables;
using Coling.API.Curriculum.Contratos.Repositorios;
using Coling.API.Curriculum.Modelo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Implementacion.Repositorios
{
    public class ProfesionRepositorio : IProfesionRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string? tablaNombre;
        private readonly IConfiguration configuration;
        public ProfesionRepositorio(IConfiguration conf)
        {
            this.configuration = conf;
            this.cadenaConexion = configuration.GetSection("cadenaConexion").Value;
            this.tablaNombre = "Profesion";
        }

        public async Task<bool> Create(Profesion objeto)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpsertEntityAsync(objeto);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(string partitionKey, string rowkey)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.DeleteEntityAsync(partitionKey, rowkey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Profesion> Get(string id)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Educacion' and RowKey eq '{id}'";
            await foreach (Profesion linea in tablaCliente.QueryAsync<Profesion>(filter: filtro))
            {
                return linea;
            }
            return null;
        }

        public async Task<List<Profesion>> GetAll()
        {
            List<Profesion> lista = new List<Profesion>();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Educacion'";
            await foreach (Profesion linea in tablaCliente.QueryAsync<Profesion>(filter: filtro))
            {
                lista.Add(linea);
            }
            return lista;
        }

        public async Task<bool> Update(Profesion objeto)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpdateEntityAsync(objeto, objeto.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
