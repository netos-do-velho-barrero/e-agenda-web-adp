using System.ComponentModel.DataAnnotations;
using eAgenda.WebApp.Modulos.ModuloCompromissos.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloCompromissos.Apresentacao;

public record ListarCompromissosViewModel(
    Guid Id,
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso Tipo
);

public record CadastrarCompromissoViewModel(
    [Required(ErrorMessage = "O campo \"Assunto\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Assunto\" deve conter entre 2 e 100 caracteres.")]
    string Assunto,

    [Required(ErrorMessage = "O campo \"Data de Ocorrencia\" deve ser preenchido.")]
    DateOnly DataOcorrencia,

    [Required(ErrorMessage = "O campo \"Hora de Inicio\" deve ser preenchido.")]
    TimeOnly HoraInicio,

    [Required(ErrorMessage = "O campo \"Hora de Termino\" deve ser preenchido.")]
    TimeOnly HoraTermino,

    [Required(ErrorMessage = "O campo \"Tipo\" deve ser preenchido.")]
    TipoCompromisso Tipo,

    string? Local,
    string? Link,
    Guid? ContatoId
);

public record EditarCompromissoViewModel(
    Guid Id,

    [Required(ErrorMessage = "O campo \"Assunto\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Assunto\" deve conter entre 2 e 100 caracteres.")]
    string Assunto,

    [Required(ErrorMessage = "O campo \"Data de Ocorrencia\" deve ser preenchido.")]
    DateOnly DataOcorrencia,

    [Required(ErrorMessage = "O campo \"Hora de Inicio\" deve ser preenchido.")]
    TimeOnly HoraInicio,

    [Required(ErrorMessage = "O campo \"Hora de Termino\" deve ser preenchido.")]
    TimeOnly HoraTermino,

    [Required(ErrorMessage = "O campo \"Tipo\" deve ser preenchido.")]
    TipoCompromisso Tipo,

    string? Local,
    string? Link,
    Guid? ContatoId
);

public record ExcluirCompromissoViewModel(
    Guid Id,
    string Assunto,
    DateOnly DataOcorrencia,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso Tipo,
    string? Local,
    string? Link,
    Guid? ContatoId
);