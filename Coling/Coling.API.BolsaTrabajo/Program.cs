using Coling.API.BolsaTrabajo.Contratos;
using Coling.API.BolsaTrabajo.Implementacion;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<ISolicitudLogic, SolicitudLogic>();
        services.AddScoped<IOfertaLaboralLogic, OfertaLaboralLogic>();

    })
    .Build();

host.Run();
