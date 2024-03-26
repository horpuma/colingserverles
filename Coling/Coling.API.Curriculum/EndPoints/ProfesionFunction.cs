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
    public class ProfesionFunction
    {
        private readonly ILogger<ProfesionFunction> _logger;
        private readonly IProfesionRepositorio repos;

        public ProfesionFunction(ILogger<ProfesionFunction> logger, IProfesionRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("InsertarProfesion")]
        [OpenApiOperation("Insertarspec", "InsertarProfesion", Description = " Sirve para ingresar una Profesion")]
        [OpenApiRequestBody("application/json", typeof(Profesion),
            Description = "Ingresar Profesion nueva")]
        /*[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", 
            bodyType: typeof(List<Profesion>), 
            Description = "Mostrara una lista de Profesions")]*/
        public async Task<HttpResponseData> InsertarProfesion([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Profesion>() ?? throw new Exception("Debe ingreesar una Profesion con todos los datos");
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

        [Function("ListarProfesion")]
        [OpenApiOperation("Listarspec", "ListarProfesion", Description = " Sirve para listar todas las Profesiones")]

        public async Task<HttpResponseData> ListarProfesion([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
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
        [Function("ListarProfesionId")]
        [OpenApiOperation("Listaridspec", "ListarProfesionId", Description = " Sirve para listar una Profesion por id rowkey")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de la Profesion", Description = "El RowKey de la Profesion a obtener", Visibility = OpenApiVisibilityType.Important)]
        /*[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(List<Profesion>),
            Description = "Mostrara una lista de Profesions")]*/
        public async Task<HttpResponseData> ListarProfesionId([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listarProfesions/{id}")] HttpRequestData req, string id)
        {
            HttpResponseData respuesta;
            try
            {
                var ProfesionId = repos.Get(id);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(ProfesionId.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }

        }
        [Function("ModificarProfesion")]
        [OpenApiOperation("Modificarspec", "ModificarProfesion", Description = " Sirve para editar una Profesion")]
        [OpenApiRequestBody("application/json", typeof(Profesion),
            Description = "editar Profesion")]
        public async Task<HttpResponseData> ModificarProfesion(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarProfesion")] HttpRequestData req)
        {
            HttpResponseData respuesta;

            _logger.LogInformation($"Ejecutando azure function para modificar Profesion.");
            try
            {
                var editar = await req.ReadFromJsonAsync<Profesion>() ?? throw new Exception("Debe ingresar una Profesion con todos sus datos");
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


        [Function("EliminarProfesion")]
        [OpenApiOperation("Eliminarspec", "DeleteProfesion", Description = " Sirve para eliminar una Profesion")]
        [OpenApiParameter(name: "partitionKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "PartitionKey de la Profesion", Description = "El PartitionKey de la Profesion a borrar", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "RowKey de la Profesion", Description = "El RowKey de la Profesion a borrar", Visibility = OpenApiVisibilityType.Important)]

        /*[OpenApiRequestBody("application/json", typeof(Profesion),
            Description = "Ingresar Profesion nueva")]*/
        public async Task<HttpResponseData> EliminarProfesion(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminarProfesion/{partitionKey},{rowkey}")] HttpRequestData req,
        string partitionKey, string rowkey)
        {
            HttpResponseData respuesta;
            _logger.LogInformation($"Ejecutando azure function para eliminar Profesion con rowkey: {rowkey} y partitionkey {partitionKey}.");
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
