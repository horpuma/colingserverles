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
    public class TipoSocialFunction
    {
        private readonly ILogger<TipoSocialFunction> _logger;
        private readonly ITipoSocialLogic tipoSocialLogic;

        public TipoSocialFunction(ILogger<TipoSocialFunction> logger, ITipoSocialLogic tipoSocialLogic)
        {
            _logger = logger;
            this.tipoSocialLogic = tipoSocialLogic;
        }

        [Function("ListarTipoSociales")]
        public async Task<HttpResponseData> ListarTipoSociales([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listartiposociales")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para listar TipoSocial.");
            try
            {
                var listaTipoSociales = tipoSocialLogic.ListarTipoSocialTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaTipoSociales.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("ListarTipoSocialId")]
        public async Task<HttpResponseData> ListarTipoSocialId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listartiposociales/{id:int}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando azure function para listar un TipoSocial por ID.");
            try
            {
                var tipoSocialId = tipoSocialLogic.ObtenerTipoSocialById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(tipoSocialId.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("InsertarTipoSocial")]
        public async Task<HttpResponseData> InsertarTipoSocial([HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertartiposocial")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar TipoSocial.");
            try
            {
                var ts = await req.ReadFromJsonAsync<TipoSocial>() ?? throw new Exception("Debe ingresar un TipoSocial con todos sus datos");
                bool seGuardo = await tipoSocialLogic.InsertarTipoSocial(ts);
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

        [Function("ModificarTipoSocial")]
        public async Task<HttpResponseData> ModificarTipoSocial(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificartiposocial/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar TipoSocial con Id: {id}.");
            try
            {
                var tipoSocial = await req.ReadFromJsonAsync<TipoSocial>() ?? throw new Exception("Debe ingresar un TipoSocial con todos sus datos");
                bool modificado = await tipoSocialLogic.ModificarTipoSocial(tipoSocial, id);

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

        [Function("EliminarTipoSocial")]
        public async Task<HttpResponseData> EliminarTipoSocial(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminartiposocial/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar TipoSocial con Id: {id}.");
            try
            {
                bool eliminado = await tipoSocialLogic.EliminarTipoSocial(id);

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
