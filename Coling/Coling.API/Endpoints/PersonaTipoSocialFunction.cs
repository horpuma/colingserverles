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
    public class PersonaTipoSocialFunction
    {
        private readonly ILogger<PersonaTipoSocialFunction> _logger;
        private readonly IPersonaTipoSocialLogic personaTipoSocialLogic;

        public PersonaTipoSocialFunction(ILogger<PersonaTipoSocialFunction> logger, IPersonaTipoSocialLogic personaTipoSocialLogic)
        {
            _logger = logger;
            this.personaTipoSocialLogic = personaTipoSocialLogic;
        }

        [Function("ListarPersonaTipoSociales")]
        public async Task<HttpResponseData> ListarPersonaTipoSociales([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarpersonatiposociales")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para listar PersonaTipoSocial.");
            try
            {
                var listaPersonaTipoSociales = personaTipoSocialLogic.ListarPersonaTipoSocialTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaPersonaTipoSociales.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("ListarPersonaTipoSocialId")]
        public async Task<HttpResponseData> ListarPersonaTipoSocialId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarpersonatiposociales/{id:int}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando azure function para listar un PersonaTipoSocial por ID.");
            try
            {
                var personaTipoSocialId = personaTipoSocialLogic.ObtenerPersonaTipoSocialById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(personaTipoSocialId.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

        [Function("InsertarPersonaTipoSocial")]
        public async Task<HttpResponseData> InsertarPersonaTipoSocial([HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertarpersonatiposocial")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar PersonaTipoSociales.");
            try
            {
                var pts = await req.ReadFromJsonAsync<PersonaTipoSocial>() ?? throw new Exception("Debe ingresar un PersonaTipoSocial con todos sus datos");
                bool seGuardo = await personaTipoSocialLogic.InsertarPersonaTipoSocial(pts);
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

        [Function("ModificarPersonaTipoSocial")]
        public async Task<HttpResponseData> ModificarPersonaTipoSocial(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarpersonatiposocial/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar PersonaTipoSocial con Id: {id}.");
            try
            {
                var personaTipoSocial = await req.ReadFromJsonAsync<PersonaTipoSocial>() ?? throw new Exception("Debe ingresar un PersonaTipoSocial con todos sus datos");
                bool modificado = await personaTipoSocialLogic.ModificarPersonaTipoSocial(personaTipoSocial, id);

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


        [Function("EliminarPersonaTipoSocial")]
        public async Task<HttpResponseData> EliminarPersonaTipoSocial(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarpersonatiposocial/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar PersonaTipoSocial con Id: {id}.");
            try
            {
                bool eliminado = await personaTipoSocialLogic.EliminarPersonaTipoSocial(id);

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
