using eAgenda.WebApp.Compartilhado.Infra.Sql;
using eAgenda.WebApp.Modulos.ModuloCompromissos.Dominio;
using eAgenda.WebApp.Modulos.ModuloCompromissos.Infra;
using eAgenda.WebApp.Modulos.ModuloContatos.Dominio;
using eAgenda.WebApp.Modulos.ModuloContatos.Infra;

namespace eAgenda.WebApp.Compartilhado.Infra;

public static class InjecaoDependencia
{
    public static void AddInfraRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IRepositorioContato, RepositorioContatoEmSql>();
        services.AddScoped<IRepositorioCompromisso, RepositorioCompromissoEmSql>();
    }
}