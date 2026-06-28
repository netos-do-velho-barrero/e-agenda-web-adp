using System.ComponentModel.DataAnnotations;
using eAgenda.WebApp.Modulos.ModuloDespesas.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloDespesas.Apresentacao;

public record ListarDespesaViewModel(
    Guid Id, 
    string Descricao, 
    DateTime DataOcorrencia, 
    decimal Valor, 
    FormaPagamento FormaPagamento
);

public record CadastrarDespesaViewModel(
    [Required(ErrorMessage = "O campo 'Descrição' é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo 'Descrição' deve conter entre 2 e 100 caracteres.")]
    string Descricao,

    [Required(ErrorMessage = "O campo 'Data' é obrigatório.")]
    DateTime DataOcorrencia,

    [Range(0.01, double.MaxValue, ErrorMessage = "O campo 'Valor' deve ser maior que zero.")]
    decimal Valor,

    [Required(ErrorMessage = "O campo 'Forma de Pagamento' é obrigatório.")]
    FormaPagamento FormaPagamento,

    [Required(ErrorMessage = "A despesa deve possuir ao menos uma categoria.")]
    List<Guid> CategoriaIds
)
{
    public CadastrarDespesaViewModel() : this(
        string.Empty, 
        DateTime.Today, 
        0, 
        FormaPagamento.AVista, 
        []
    ) { }
}

public record EditarDespesaViewModel(
    Guid Id,

    [Required(ErrorMessage = "O campo 'Descrição' é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo 'Descrição' deve conter entre 2 e 100 caracteres.")]
    string Descricao,

    [Required(ErrorMessage = "O campo 'Data' é obrigatório.")]
    DateTime DataOcorrencia,

    [Range(0.01, double.MaxValue, ErrorMessage = "O campo 'Valor' deve ser maior que zero.")]
    decimal Valor,

    [Required(ErrorMessage = "O campo 'Forma de Pagamento' é obrigatório.")]
    FormaPagamento FormaPagamento,

    [Required(ErrorMessage = "A despesa deve possuir ao menos uma categoria.")]
    List<Guid> CategoriaIds
);

public record ExcluirDespesaViewModel(
    Guid Id, 
    string Descricao
);