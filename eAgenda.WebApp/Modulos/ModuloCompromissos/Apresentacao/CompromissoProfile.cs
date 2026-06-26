using AutoMapper;
using eAgenda.WebApp.Modulos.ModuloCompromissos.Aplicacao;

namespace eAgenda.WebApp.Modulos.ModuloCompromissos.Apresentacao;

public class CompromissoProfile : Profile
{
    public CompromissoProfile()
    {
        CreateMap<ListarCompromissosDto, ListarCompromissosViewModel>();

        CreateMap<CadastrarCompromissoViewModel, CadastrarCompromissoDto>();
        CreateMap<EditarCompromissoViewModel, EditarCompromissoDto>();

        CreateMap<DetalhesCompromissoDto, EditarCompromissoViewModel>();
        CreateMap<DetalhesCompromissoDto, ExcluirCompromissoViewModel>();
    }
}