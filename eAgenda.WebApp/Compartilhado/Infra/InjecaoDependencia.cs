using eAgenda.WebApp.Compartilhado.Infra.Sql;
using eAgenda.WebApp.Modulos.ModuloCategoria.Dominio;
using eAgenda.WebApp.Modulos.ModuloCategoria.Infra;
using eAgenda.WebApp.Modulos.ModuloCompromissos.Dominio;
using eAgenda.WebApp.Modulos.ModuloCompromissos.Infra;
using eAgenda.WebApp.Modulos.ModuloContatos.Dominio;
using eAgenda.WebApp.Modulos.ModuloContatos.Infra;
using eAgenda.WebApp.Modulos.ModuloDespesas.Dominio;
using eAgenda.WebApp.Modulos.ModuloDespesas.Infra;
using eAgenda.WebApp.Modulos.ModuloTarefas.Dominio;
using eAgenda.WebApp.Modulos.ModuloTarefas.Infra;

namespace eAgenda.WebApp.Compartilhado.Infra;

public static class InjecaoDependencia
{
    public static void AddInfraRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IRepositorioContato, RepositorioContatoEmSql>();
        services.AddScoped<IRepositorioCompromisso, RepositorioCompromissoEmSql>();
        services.AddScoped<IRepositorioTarefa, RepositorioTarefaEmSql>();
        services.AddScoped<IRepositorioCategoria, RepositorioCategoriaEmSql>();
        services.AddScoped<IRepositorioDespesa, RepositorioDespesaEmSql>();
    }
}