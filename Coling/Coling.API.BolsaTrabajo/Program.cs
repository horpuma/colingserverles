using Coling.API.BolsaTrabajo.Contratos;
using Coling.API.BolsaTrabajo.Implementacion;
using Coling.Utilitarios.Middlewares;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    //.ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<ISolicitudLogic, SolicitudLogic>();
        services.AddScoped<IOfertaLaboralLogic, OfertaLaboralLogic>();

    }).ConfigureFunctionsWebApplication(x =>
{
    x.UseMiddleware<JwtMiddleware>();
})
    .Build();

host.Run();
