using AutoMapper;
using eAgenda.WebApp.Modulos.ModuloDespesas.Aplicacao;

namespace eAgenda.WebApp.Modulos.ModuloDespesas.Apresentacao;

public class DespesaProfile : Profile
{
    public DespesaProfile()
    {
        CreateMap<ListarDespesaDto, ListarDespesaViewModel>();
        CreateMap<CadastrarDespesaViewModel, CadastrarDespesaDto>();
        CreateMap<EditarDespesaViewModel, EditarDespesaDto>(); 
        CreateMap<DetalhesDespesaDto, EditarDespesaViewModel>();
        CreateMap<DetalhesDespesaDto, ExcluirDespesaViewModel>();
    }
}