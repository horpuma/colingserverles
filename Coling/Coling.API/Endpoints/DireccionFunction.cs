using Coling.API.Afiliados.Contratos;
using Coling.API.Afiliados.Implementacion;
using ColingShared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Coling.API.Afiliados.Endpoints
{
    public class DireccionFunction
    {
        private readonly ILogger<DireccionFunction> _logger;
        private readonly IDireccionLogic direccionLogic;

        public DireccionFunction(ILogger<DireccionFunction> logger, IDireccionLogic direccionLogic)
        {
            _logger = logger;
            this.direccionLogic = direccionLogic;
        }

        [Function("ListarDirecciones")]
        public async Task<HttpResponseData> ListarDirecciones([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listardirecciones")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para listar direcciones.");
            try
            {
                var listaDirecciones = direccionLogic.ListarDireccionTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaDirecciones.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }
        [Function("ListarDireccionId")]
        public async Task<HttpResponseData> ListarDireccionId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listardirecciones/{id:int}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando azure function para listar una direccion por ID.");
            try
            {
                var direccionId = direccionLogic.ObtenerDireccionById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(direccionId.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("InsertarDireccion")]
        public async Task<HttpResponseData> InsertarDireccion([HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertardireccion")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar direcciones.");
            try
            {
                var dir = await req.ReadFromJsonAsync<Direccion>() ?? throw new Exception("Debe ingresar una direccion con todos sus datos");
                bool seGuardo = await direccionLogic.InsertarDireccion(dir);
                if (seGuardo)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("ModificarDireccion")]
        public async Task<HttpResponseData> ModificarDireccion(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificardireccion/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar direccion con Id: {id}.");
            try
            {
                var direccion = await req.ReadFromJsonAsync<Direccion>() ?? throw new Exception("Debe ingresar una direccion con todos sus datos");
                bool modificado = await direccionLogic.ModificarDireccion(direccion, id);

                if (modificado)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }

                return req.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }


        [Function("EliminarDireccion")]
        public async Task<HttpResponseData> EliminarDireccion(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminardireccion/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar direccion con Id: {id}.");
            try
            {
                bool eliminado = await direccionLogic.EliminarDireccion(id);

                if (eliminado)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }

                return req.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

    }
}
