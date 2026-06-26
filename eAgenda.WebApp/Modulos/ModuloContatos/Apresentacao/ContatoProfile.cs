using AutoMapper;
using eAgenda.WebApp.Modulos.ModuloContatos.Aplicacao;

namespace eAgenda.WebApp.Modulos.ModuloContatos.Apresentacao;

public class ContatoProfile : Profile
{
    public ContatoProfile()
    {
        CreateMap<ListarContatosDto, ListarContatosViewModel>();

        CreateMap<CadastrarContatoViewModel, CadastrarContatoDto>();
        CreateMap<EditarContatoViewModel, EditarContatoDto>();

        CreateMap<DetalhesContatoDto, EditarContatoViewModel>();
        CreateMap<DetalhesContatoDto, ExcluirContatoViewModel>();
    }
}