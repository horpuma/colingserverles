using Coling.API.Curriculum.Contratos.Repositorios;
using Coling.API.Curriculum.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Coling.API.Curriculum.EndPoints
{
    public class ExperienciaLaboralFunction
    {
        private readonly ILogger<ExperienciaLaboralFunction> _logger;
        private readonly IExperienciaLaboralRepositorio repos;

        public ExperienciaLaboralFunction(ILogger<ExperienciaLaboralFunction> logger, IExperienciaLaboralRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("InsertarExperienciaLaboral")]
        public async Task<HttpResponseData> InsertarExperienciaLaboral([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<ExperienciaLaboral>() ?? throw new Exception("Debe ingreesar una ExperienciaLaboral con todos los datos");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.UtcNow;
                bool sw = await repos.Create(registro);
                if (sw)
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                else
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                return respuesta;

            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }

        }

        [Function("ListarExperienciaLaboral")]
        public async Task<HttpResponseData> ListarExperienciaLaboral([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var lista = repos.GetAll();
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(lista.Result);
                return respuesta;

            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }

        }
        [Function("ListarExperienciaLaboralId")]
        public async Task<HttpResponseData> ListarExperienciaLaboralId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarExperienciaLaborals/{id}")] HttpRequestData req, string id)
        {
            HttpResponseData respuesta;
            try
            {
                var ExperienciaLaboralId = repos.Get(id);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(ExperienciaLaboralId.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }

        }
        [Function("ModificarExperienciaLaboral")]
        public async Task<HttpResponseData> ModificarExperienciaLaboral(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarExperienciaLaboral")] HttpRequestData req)
        {
            HttpResponseData respuesta;

            _logger.LogInformation($"Ejecutando azure function para modificar ExperienciaLaboral.");
            try
            {
                var editar = await req.ReadFromJsonAsync<ExperienciaLaboral>() ?? throw new Exception("Debe ingresar una ExperienciaLaboral con todos sus datos");
                bool modificado = await repos.Update(editar);
                if (modificado)
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                else
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }


        [Function("EliminarExperienciaLaboral")]
        public async Task<HttpResponseData> EliminarExperienciaLaboral(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarExperienciaLaboral/{partitionKey},{rowkey}")] HttpRequestData req,
        string partitionKey, string rowkey)
        {
            HttpResponseData respuesta;
            _logger.LogInformation($"Ejecutando azure function para eliminar ExperienciaLaboral con rowkey: {rowkey} y partitionkey {partitionKey}.");
            try
            {
                bool eliminado = await repos.Delete(partitionKey, rowkey);

                if (eliminado)
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                else
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
    }
}
