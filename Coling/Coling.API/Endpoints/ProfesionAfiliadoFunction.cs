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
    public class ProfesionAfiliadoFunction
    {
        private readonly ILogger<ProfesionAfiliadoFunction> _logger;
        private readonly IProfesionAfiliadoLogic profesionAfiliadoLogic;

        public ProfesionAfiliadoFunction(ILogger<ProfesionAfiliadoFunction> logger, IProfesionAfiliadoLogic profesionAfiliadoLogic)
        {
            _logger = logger;
            this.profesionAfiliadoLogic = profesionAfiliadoLogic;
        }

        [Function("ListarProfesionAfiliados")]
        public async Task<HttpResponseData> ListarProfesionAfiliados([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarprofesionafiliados")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para listar ProfesionAfiliado.");
            try
            {
                var listaProfesionAfiliados = profesionAfiliadoLogic.ListarProfesionAfiliadoTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaProfesionAfiliados.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("ListarProfesionAfiliadoId")]
        public async Task<HttpResponseData> ListarProfesionAfiliadoId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarprofesionafiliados/{id:int}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando azure function para listar un ProfesionAfiliado por ID.");
            try
            {
                var profesionAfiliadoId = profesionAfiliadoLogic.ObtenerProfesionAfiliadoById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(profesionAfiliadoId.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("InsertarProfesionAfiliado")]
        public async Task<HttpResponseData> InsertarProfesionAfiliado([HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertarprofesionafiliado")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar ProfesionAfiliados.");
            try
            {
                var pfa = await req.ReadFromJsonAsync<ProfesionAfiliado>() ?? throw new Exception("Debe ingresar un ProfesionAfiliado con todos sus datos");
                bool seGuardo = await profesionAfiliadoLogic.InsertarProfesionAfiliado(pfa);
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

        [Function("ModificarProfesionAfiliado")]
        public async Task<HttpResponseData> ModificarProfesionAfiliado(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarprofesionafiliado/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar ProfesionAfiliado con Id: {id}.");
            try
            {
                var profesionAfiliado = await req.ReadFromJsonAsync<ProfesionAfiliado>() ?? throw new Exception("Debe ingresar un ProfesionAfiliado con todos sus datos");
                bool modificado = await profesionAfiliadoLogic.ModificarProfesionAfiliado(profesionAfiliado, id);

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


        [Function("EliminarProfesionAfiliado")]
        public async Task<HttpResponseData> EliminarProfesionAfiliado(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarprofesionafiliado/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar ProfesionAfiliado con Id: {id}.");
            try
            {
                bool eliminado = await profesionAfiliadoLogic.EliminarProfesionAfiliado(id);

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
