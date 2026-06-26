using System.ComponentModel.DataAnnotations;
using eAgenda.WebApp.Modulos.ModuloTarefas.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloTarefas.Apresentacao;

public record ItemTarefaViewModel(
    Guid Id,
    string Titulo,
    bool Concluido
);

public record ListarTarefasViewModel(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    bool Concluida,
    decimal PercentualConcluido
);

public record CadastrarTarefaViewModel(
    [Required(ErrorMessage = "O campo \"Titulo\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Titulo\" deve conter entre 2 e 100 caracteres.")]
    string Titulo,

    [Required(ErrorMessage = "O campo \"Prioridade\" deve ser preenchido.")]
    PrioridadeTarefa Prioridade,

    List<string> Itens
);

public record EditarTarefaViewModel(
    Guid Id,

    [Required(ErrorMessage = "O campo \"Titulo\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Titulo\" deve conter entre 2 e 100 caracteres.")]
    string Titulo,

    [Required(ErrorMessage = "O campo \"Prioridade\" deve ser preenchido.")]
    PrioridadeTarefa Prioridade,

    List<ItemTarefaViewModel> Itens
);

public record ExcluirTarefaViewModel(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    bool Concluida,
    decimal PercentualConcluido,
    List<ItemTarefaViewModel> Itens
);