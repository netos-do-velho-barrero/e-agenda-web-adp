using eAgenda.WebApp.Compartilhado.Aplicacao;
using eAgenda.WebApp.Compartilhado.Apresentacao;
using eAgenda.WebApp.Compartilhado.Infra;
using eAgenda.WebApp.Compartilhado.Infra.Sql;


var builder = WebApplication.CreateBuilder(args);

DapperTypeHandlers.Registrar();

builder.Services.AddInfraRepositories();
builder.Services.AddApplicationServices();
builder.Services.AddPresentationConfig();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();

