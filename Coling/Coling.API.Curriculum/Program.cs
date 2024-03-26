using Coling.API.Curriculum.Contratos.Repositorios;
using Coling.API.Curriculum.Implementacion.Repositorios;
using Coling.Utilitarios.Middlewares;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    //.ConfigureFunctionsWebApplication(/*worker => worker.UseNewtonsoftJson()*/)
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IInstitucionRepositorio, InstitucionRepositorio>();
        services.AddScoped<IEstudiosRepositorio, EstudiosRepositorio>();
        services.AddScoped<IExperienciaLaboralRepositorio, ExperienciaLaboralRepositorio>();
        services.AddScoped<IProfesionRepositorio, ProfesionRepositorio>();
        services.AddSingleton<JwtMiddleware> ();
    }).ConfigureFunctionsWebApplication(x=>
    {
        x.UseMiddleware<JwtMiddleware>();
    })
    .Build();

host.Run();
