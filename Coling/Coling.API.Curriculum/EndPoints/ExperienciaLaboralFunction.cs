using Coling.API.Curriculum.Contratos.Repositorios;
using Coling.API.Curriculum.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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
        [OpenApiOperation("Insertarspec", "InsertarExperienciaLaboral", Description = " Sirve para ingresar una ExperienciaLaboral")]
        [OpenApiRequestBody("application/json", typeof(ExperienciaLaboral),
            Description = "Ingresar ExperienciaLaboral nueva")]
        /*[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", 
            bodyType: typeof(List<ExperienciaLaboral>), 
            Description = "Mostrara una lista de ExperienciaLaborals")]*/
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
        [OpenApiOperation("Listarspec", "ListarExperinciaLaboral", Description = " Sirve para listar todas las ExperinciaLaborales")]

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
        [OpenApiOperation("Listaridspec", "ListarExperienciaLaboralId", Description = " Sirve para listar una ExperienciaLaboral por id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de la ExperienciaLaboral", Description = "El RowKey de la ExperienciaLaboral a obtener", Visibility = OpenApiVisibilityType.Important)]
        /*[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(List<ExperienciaLaboral>),
            Description = "Mostrara una lista de ExperienciaLaborals")]*/
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
        [OpenApiOperation("Modificarspec", "ModificarExperienciaLaboral", Description = " Sirve para editar una ExperienciaLaboral")]
        [OpenApiRequestBody("application/json", typeof(ExperienciaLaboral),
            Description = "editar ExperienciaLaboral")]
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
        [OpenApiOperation("Eliminarspec", "DeleteExperienciaLaboral", Description = " Sirve para eliminar una ExperienciaLaboral")]
        [OpenApiParameter(name: "partitionKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "PartitionKey de la ExperienciaLaboral", Description = "El PartitionKey de la ExperienciaLaboral a borrar", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "RowKey de la ExperienciaLaboral", Description = "El RowKey de la ExperienciaLaboral a borrar", Visibility = OpenApiVisibilityType.Important)]

        /*[OpenApiRequestBody("application/json", typeof(ExperienciaLaboral),
            Description = "Ingresar ExperienciaLaboral nueva")]*/
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
