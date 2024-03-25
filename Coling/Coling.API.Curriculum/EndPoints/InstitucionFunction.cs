using Coling.API.Curriculum.Contratos.Repositorios;
using Coling.API.Curriculum.Implementacion.Repositorios;
using Coling.API.Curriculum.Modelo;
using ColingShared;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Collections.Concurrent;
using System.Net;

namespace Coling.API.Curriculum.EndPoints
{
    public class InstitucionFunction
    {
        private readonly ILogger<InstitucionFunction> _logger;
        private readonly IInstitucionRepositorio repos;
        
        public InstitucionFunction(ILogger<InstitucionFunction> logger, IInstitucionRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("InsertarInstitucion")]
        [OpenApiOperation("Insertarspec", "InsertarInstitucion", Description = " Sirve para ingresar una institucion")]
        [OpenApiRequestBody("application/json", typeof(Institucion),
            Description ="Ingresar institucion nueva")]
        /*[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", 
            bodyType: typeof(List<Institucion>), 
            Description = "Mostrara una lista de institucions")]*/

        public async Task<HttpResponseData> InsertarInstitucion([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Institucion>() ?? throw new Exception("Debe ingreesar una institucion con todos los datos");
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

        [Function("ListarInstitucion")]
        [OpenApiOperation("Listarspec", "ListarInstitucion", Description = " Sirve para listar todas las instituciones")]
        /*[OpenApiResponseWithBody(statusCode:HttpStatusCode.OK, contentType:"application/json",bodyType: typeof(List<Institucion>),Description = "Mostrara una lista de institucions")]*/
        public async Task<HttpResponseData> ListarInstitucion([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
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
        [Function("ListarInstitucionId")]
        [OpenApiOperation("Listaridspec", "ListarInstitucionId", Description = " Sirve para listar una institucion por id")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID de la Institución", Description = "El RowKey de la institución a obtener", Visibility = OpenApiVisibilityType.Important)]
        /*[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(List<Institucion>),
            Description = "Mostrara una lista de institucions")]*/
        public async Task<HttpResponseData> ListarInstitucionId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarInstitucions/{id}")] HttpRequestData req, string id)
        {
            HttpResponseData respuesta;
            try
            {
                var institucionId = repos.Get(id);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(institucionId.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }

        }
        [Function("ModificarInstitucion")]
        [OpenApiOperation("Modificarspec", "ModificarInstitucion", Description = " Sirve para editar una institucion")]
        [OpenApiRequestBody("application/json", typeof(Institucion),
            Description = "editar institucion")]
        public async Task<HttpResponseData> ModificarInstitucion(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarInstitucion")] HttpRequestData req)
        {
            HttpResponseData respuesta;

            _logger.LogInformation($"Ejecutando azure function para modificar Institucion.");
            try
            {
                var editar = await req.ReadFromJsonAsync<Institucion>() ?? throw new Exception("Debe ingresar una institucion con todos sus datos");
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


        [Function("EliminarInstitucion")]
        [OpenApiOperation("Eliminarspec", "DeleteInstitucion", Description = " Sirve para eliminar una institucion")]
        [OpenApiParameter(name: "partitionKey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "PartitionKey de la Institución", Description = "El PartitionKey de la institución a borrar", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "RowKey de la Institución", Description = "El RowKey de la institución a borrar", Visibility = OpenApiVisibilityType.Important)]

        /*[OpenApiRequestBody("application/json", typeof(Institucion),
            Description = "Ingresar institucion nueva")]*/
        public async Task<HttpResponseData> EliminarInstitucion(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarInstitucion/{partitionKey},{rowkey}")] HttpRequestData req,
        string partitionKey, string rowkey)
        {
            HttpResponseData respuesta;
            _logger.LogInformation($"Ejecutando azure function para eliminar institucion con rowkey: {rowkey} y partitionkey {partitionKey}.");
            try
            {
                bool eliminado = await repos.Delete(partitionKey,rowkey);

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
