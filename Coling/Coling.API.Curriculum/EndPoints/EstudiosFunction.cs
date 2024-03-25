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
    public class EstudiosFunction
    {
        private readonly ILogger<EstudiosFunction> _logger;
        private readonly IEstudiosRepositorio repos;

        public EstudiosFunction(ILogger<EstudiosFunction> logger, IEstudiosRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }
        [Function("InsertarEstudios")]
        [OpenApiOperation("Insertarspec", "InsertarEstudios", Description = " Sirve para ingresar una Estudios")]
        [OpenApiRequestBody("application/json", typeof(Estudios),
            Description = "Ingresar Estudios nueva")]
        /*[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", 
            bodyType: typeof(List<Estudios>), 
            Description = "Mostrara una lista de Estudioss")]*/
        public async Task<HttpResponseData> InsertarEstudios([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Estudios>() ?? throw new Exception("Debe ingreesar una Estudios con todos los datos");
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

        [Function("ListarEstudios")]
        [OpenApiOperation("Listarspec", "ListarEstudios", Description = " Sirve para listar todas los Estudios")]

        public async Task<HttpResponseData> ListarEstudios([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
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
        [Function("ListarEstudiosId")]
        [OpenApiOperation("Listaridspec", "ListarEstudiosId", Description = " Sirve para listar una Estudios por id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de los Estudios", Description = "El RowKey de los Estudios a obtener", Visibility = OpenApiVisibilityType.Important)]
        /*[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(List<Estudios>),
            Description = "Mostrara una lista de Estudioss")]*/
        public async Task<HttpResponseData> ListarEstudiosId([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listarEstudioss/{id}")] HttpRequestData req, string id)
        {
            HttpResponseData respuesta;
            try
            {
                var EstudiosId = repos.Get(id);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(EstudiosId.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }

        }
        [Function("ModificarEstudios")]
        [OpenApiOperation("Modificarspec", "ModificarEstudios", Description = " Sirve para editar una Estudios")]
        [OpenApiRequestBody("application/json", typeof(Estudios),
            Description = "editar Estudios")]
        public async Task<HttpResponseData> ModificarEstudios(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarEstudios")] HttpRequestData req)
        {
            HttpResponseData respuesta;

            _logger.LogInformation($"Ejecutando azure function para modificar Estudios.");
            try
            {
                var editar = await req.ReadFromJsonAsync<Estudios>() ?? throw new Exception("Debe ingresar una Estudios con todos sus datos");
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


        [Function("EliminarEstudios")]
        [OpenApiOperation("Eliminarspec", "DeleteEstudios", Description = " Sirve para eliminar un Estudio")]
        [OpenApiParameter(name: "partitionKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "PartitionKey del Estudio", Description = "El PartitionKey del Estudio a borrar", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "RowKey del Estudio", Description = "El RowKey del Estudio a borrar", Visibility = OpenApiVisibilityType.Important)]

        /*[OpenApiRequestBody("application/json", typeof(Estudios),
            Description = "Ingresar Estudios nueva")]*/
        public async Task<HttpResponseData> EliminarEstudios(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminarEstudios/{partitionKey},{rowkey}")] HttpRequestData req,
        string partitionKey, string rowkey)
        {
            HttpResponseData respuesta;
            _logger.LogInformation($"Ejecutando azure function para eliminar Estudios con rowkey: {rowkey} y partitionkey {partitionKey}.");
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
