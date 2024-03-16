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
    public class AfiliadoFunction
    {
        private readonly ILogger<AfiliadoFunction> _logger;
        private readonly IAfiliadoLogic afiliadoLogic;

        public AfiliadoFunction(ILogger<AfiliadoFunction> logger, IAfiliadoLogic afiliadoLogic)
        {
            _logger = logger;
            this.afiliadoLogic = afiliadoLogic;
        }

        [Function("ListarAfiliados")]
        public async Task<HttpResponseData> ListarAfiliados([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarafiliados")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para listar afiliados.");
            try
            {
                var listaAfiliados = afiliadoLogic.ListarAfiliadoTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaAfiliados.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }
        [Function("ListarAfiliadoId")]
        public async Task<HttpResponseData> ListarAfiliadoId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarafiliados/{id:int}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando azure function para listar un afiliado por ID.");
            try
            {
                var afiliadoId = afiliadoLogic.ObtenerAfiliadoById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(afiliadoId.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("InsertarAfiliado")]
        public async Task<HttpResponseData> InsertarAfiliado([HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertarafiliado")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar afiliados.");
            try
            {
                var afi = await req.ReadFromJsonAsync<Afiliado>() ?? throw new Exception("Debe ingresar un afiliado con todos sus datos");
                bool seGuardo = await afiliadoLogic.InsertarAfiliado(afi);
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

        [Function("ModificarAfiliado")]
        public async Task<HttpResponseData> ModificarAfiliado(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarafiliado/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar afiliado con Id: {id}.");
            try
            {
                var afiliado = await req.ReadFromJsonAsync<Afiliado>() ?? throw new Exception("Debe ingresar un afiliado con todos sus datos");
                bool modificado = await afiliadoLogic.ModificarAfiliado(afiliado, id);

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


        [Function("EliminarAfiliado")]
        public async Task<HttpResponseData> EliminarAfiliado(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarafiliado/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar afiliado con Id: {id}.");
            try
            {
                bool eliminado = await afiliadoLogic.EliminarAfiliado(id);

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
