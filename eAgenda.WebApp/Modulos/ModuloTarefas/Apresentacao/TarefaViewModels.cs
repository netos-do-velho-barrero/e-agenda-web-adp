using System.ComponentModel.DataAnnotations;
using eAgenda.WebApp.Modulos.ModuloTarefas.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloTarefas.Apresentacao;

public class ItemTarefaViewModel
{
    public Guid Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public bool Concluido { get; set; }
}

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

public class EditarTarefaViewModel
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Titulo { get; set; } = string.Empty;

    public PrioridadeTarefa Prioridade { get; set; }

    public List<ItemTarefaViewModel> Itens { get; set; } = [];
}

public record ExcluirTarefaViewModel(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    bool Concluida,
    decimal PercentualConcluido,
    List<ItemTarefaViewModel> Itens
);