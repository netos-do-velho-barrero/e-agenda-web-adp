using eAgenda.WebApp.Modulos.ModuloContatos.Aplicacao;

namespace eAgenda.WebApp.Compartilhado.Aplicacao;

public static class InjecaoDependencia
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ServicoContato>();
    }
}