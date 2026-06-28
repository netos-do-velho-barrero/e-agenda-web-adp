using eAgenda.WebApp.Modulos.ModuloCategoria.Dominio;
namespace eAgenda.WebApp.Modulos.ModuloCategoria.Aplicacao;

public record ListarCategoriaDto(
    Guid Id,
    string Titulo
    );

public record CadastrarCategoriaDto(
    string Titulo
    );

public record EditarCategoriaDto(
    Guid Id,
    string Titulo
    );

public record DetalhesCategoriaDto(
    Guid Id,
    string Titulo
);