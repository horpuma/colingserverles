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
    public class TelefonoFunction
    {
        private readonly ILogger<TelefonoFunction> _logger;
        private readonly ITelefonoLogic telefonoLogic;

        public TelefonoFunction(ILogger<TelefonoFunction> logger, ITelefonoLogic telefonoLogic)
        {
            _logger = logger;
            this.telefonoLogic = telefonoLogic;
        }

        [Function("ListarTelefonos")]
        public async Task<HttpResponseData> ListarTelefonos([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listartelefonos")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para listar telefonos.");
            try
            {
                var listaTelefonos = telefonoLogic.ListarTelefonoTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaTelefonos.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }
        [Function("ListarTelefonoId")]
        public async Task<HttpResponseData> ListarTelefonoId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listartelefonos/{id:int}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando azure function para listar un telefono por ID.");
            try
            {
                var telefonoId = telefonoLogic.ObtenerTelefonoById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(telefonoId.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("InsertarTelefono")]
        public async Task<HttpResponseData> InsertarTelefono([HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertartelefono")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar telefonos.");
            try
            {
                var tel = await req.ReadFromJsonAsync<Telefono>() ?? throw new Exception("Debe ingresar un telefono con todos sus datos");
                bool seGuardo = await telefonoLogic.InsertarTelefono(tel);
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

        [Function("ModificarTelefono")]
        public async Task<HttpResponseData> ModificarTelefono(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificartelefono/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar telefono con Id: {id}.");
            try
            {
                var telefono = await req.ReadFromJsonAsync<Telefono>() ?? throw new Exception("Debe ingresar un telefono con todos sus datos");
                bool modificado = await telefonoLogic.ModificarTelefono(telefono, id);

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


        [Function("EliminarTelefono")]
        public async Task<HttpResponseData> EliminarTelefono(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminartelefono/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar telefono con Id: {id}.");
            try
            {
                bool eliminado = await telefonoLogic.EliminarTelefono(id);

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
