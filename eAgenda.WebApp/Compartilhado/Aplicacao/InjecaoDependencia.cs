using eAgenda.WebApp.Modulos.ModuloCategorias.Aplicacao;
using eAgenda.WebApp.Modulos.ModuloCompromissos.Aplicacao;
using eAgenda.WebApp.Modulos.ModuloContatos.Aplicacao;
using eAgenda.WebApp.Modulos.ModuloDespesas.Aplicacao;
using eAgenda.WebApp.Modulos.ModuloTarefas.Aplicacao;

namespace eAgenda.WebApp.Compartilhado.Aplicacao;

public static class InjecaoDependencia
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ServicoContato>();
        services.AddScoped<ServicoCategoria>();
        services.AddScoped<ServicoCompromisso>();
        services.AddScoped<ServicoDespesa>();
        services.AddScoped<ServicoTarefa>();
    }
}