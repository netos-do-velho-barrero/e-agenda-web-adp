using eAgenda.WebApp.Modulos.ModuloTarefas.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloTarefas.Aplicacao;

public record ItemTarefaDto(
    Guid Id,
    string Titulo,
    bool Concluido
);

public record ListarTarefasDto(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    bool Concluida,
    decimal PercentualConcluido
);

public record CadastrarTarefaDto(
    string Titulo,
    PrioridadeTarefa Prioridade,
    List<string> Itens
);

public record EditarTarefaDto(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    List<ItemTarefaDto> Itens
);

public record DetalhesTarefaDto(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    bool Concluida,
    decimal PercentualConcluido,
    List<ItemTarefaDto> Itens
);