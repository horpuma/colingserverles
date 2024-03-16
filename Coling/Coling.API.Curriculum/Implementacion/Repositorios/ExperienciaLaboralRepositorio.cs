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
    public class ExperienciaLaboralRepositorio : IExperienciaLaboralRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string? tablaNombre;
        private readonly IConfiguration configuration;
        public ExperienciaLaboralRepositorio(IConfiguration conf)
        {
            this.configuration = conf;
            this.cadenaConexion = configuration.GetSection("cadenaConexion").Value;
            this.tablaNombre = "ExperienciaLaboral";
        }

        public async Task<bool> Create(ExperienciaLaboral objeto)
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

        public async Task<ExperienciaLaboral> Get(string id)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Educacion' and RowKey eq '{id}'";
            await foreach (ExperienciaLaboral linea in tablaCliente.QueryAsync<ExperienciaLaboral>(filter: filtro))
            {
                return linea;
            }
            return null;
        }

        public async Task<List<ExperienciaLaboral>> GetAll()
        {
            List<ExperienciaLaboral> lista = new List<ExperienciaLaboral>();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Educacion'";
            await foreach (ExperienciaLaboral linea in tablaCliente.QueryAsync<ExperienciaLaboral>(filter: filtro))
            {
                lista.Add(linea);
            }
            return lista;
        }

        public async Task<bool> Update(ExperienciaLaboral objeto)
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
