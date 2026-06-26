
namespace eAgenda.WebApp.Modulos.ModuloCategorias.Aplicacao;

public record ListarCategoriasDto(
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