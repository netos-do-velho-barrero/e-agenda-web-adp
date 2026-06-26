using eAgenda.WebApp.Compartilhado.Aplicacao;
using eAgenda.WebApp.Compartilhado.Apresentacao;
using eAgenda.WebApp.Compartilhado.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfraRepositories();
builder.Services.AddApplicationServices();
builder.Services.AddPresentationConfig();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();