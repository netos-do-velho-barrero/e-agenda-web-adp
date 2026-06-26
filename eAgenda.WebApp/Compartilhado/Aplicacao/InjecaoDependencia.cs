using AutoMapper;
using eAgenda.WebApp.Modulos.ModuloContatos.Aplicacao;
using eAgenda.WebApp.Modulos.ModuloCompromissos.Aplicacao;
using eAgenda.WebApp.Modulos.ModuloDespesas.Aplicacao;
using eAgenda.WebApp.Modulos.ModuloTarefas.Aplicacao;
using eAgenda.WebApp.Modulos.ModuloCategoria.Aplicacao;
using eAgenda.WebApp.Modulos.ModuloCategoria.Apresentacao;
using eAgenda.WebApp.Modulos.ModuloCompromissos.Apresentacao;
using eAgenda.WebApp.Modulos.ModuloContatos.Apresentacao;
using eAgenda.WebApp.Modulos.ModuloTarefas.Apresentacao;

namespace eAgenda.WebApp.Compartilhado.Aplicacao;

public static class InjecaoDependencia
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
       
        services.AddScoped<ServicoContato>();
        services.AddScoped<ServicoCompromisso>();
        services.AddScoped<ServicoCategoria>();
        services.AddScoped<ServicoDespesa>();
        services.AddScoped<ServicoTarefa>();

        
        services.AddControllersWithViews().AddRazorOptions(options =>
        {
            options.ViewLocationFormats.Clear();

            options.ViewLocationFormats.Add("/Modulos/Modulo{1}s/Apresentacao/Views/{0}.cshtml");

            options.ViewLocationFormats.Add("/Compartilhado/Apresentacao/Views/{0}.cshtml");
        });

    
        services.AddAutoMapper(config =>
        {
           config.AddProfile<ContatoProfile>();
            config.AddProfile<CompromissoProfile>();
            config.AddProfile<CategoriaProfile>();
          // config.AddProfile<DespesaProfile>();
            config.AddProfile<TarefaProfile>();
        });
    }
}