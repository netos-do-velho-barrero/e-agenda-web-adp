using AutoMapper;
using eAgenda.WebApp.Modulos.ModuloTarefas.Aplicacao;

namespace eAgenda.WebApp.Modulos.ModuloTarefas.Apresentacao;

public class TarefaProfile : Profile
{
    public TarefaProfile()
    {
        CreateMap<ItemTarefaDto, ItemTarefaViewModel>();
        CreateMap<ItemTarefaViewModel, ItemTarefaDto>();

        CreateMap<ListarTarefasDto, ListarTarefasViewModel>();

        CreateMap<CadastrarTarefaViewModel, CadastrarTarefaDto>();
        CreateMap<EditarTarefaViewModel, EditarTarefaDto>();

        CreateMap<DetalhesTarefaDto, EditarTarefaViewModel>();
        CreateMap<DetalhesTarefaDto, ExcluirTarefaViewModel>();
    }
}