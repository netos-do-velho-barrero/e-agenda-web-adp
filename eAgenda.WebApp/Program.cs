using eAgenda.WebApp.Compartilhado.Aplicacao;
using eAgenda.WebApp.Compartilhado.Apresentacao;
using eAgenda.WebApp.Compartilhado.Infra;
using eAgenda.WebApp.Compartilhado.Infra.Sql;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddUserSecrets<Program>();
}

DapperTypeHandlers.Registrar();

builder.Services.AddInfraRepositories();
builder.Services.AddApplicationServices(builder.Configuration, builder.Logging);
builder.Services.AddPresentationConfig(builder.Configuration);

// Health Check
builder.Services.AddHealthChecks()
    .AddCheck<SqlServerHealthCheck>("sqlserver-db-check", tags: ["ready"]);

var app = builder.Build();

app.MapMethods("/health", new[] { "HEAD" }, () =>
{
    // Sem corpo na resposta (HEAD)
    return Results.Ok();
});

app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();

