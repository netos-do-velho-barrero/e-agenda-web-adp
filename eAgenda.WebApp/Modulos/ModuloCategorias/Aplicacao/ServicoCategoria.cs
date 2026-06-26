using FluentResults;
using eAgenda.WebApp.Modulos.ModuloCategorias.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloCategorias.Aplicacao;

public class ServicoCategoria
{
    private readonly IRepositorioCategoria repositorioCategoria;

    public ServicoCategoria(IRepositorioCategoria repositorioCategoria)
    {
        this.repositorioCategoria = repositorioCategoria;
    }

    public Result Cadastrar(CadastrarCategoriaDto dto)
    {
        if (repositorioCategoria.ExisteTitulo(dto.Titulo))
            return Falha(nameof(dto.Titulo), "Ja existe uma categoria com este titulo.");

        Categoria categoria = new Categoria(dto.Titulo);

        Result resultadoValidacao = ValidarEntidade(categoria);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioCategoria.Cadastrar(categoria);

        return Result.Ok();
    }

    public Result Editar(EditarCategoriaDto dto)
    {
        if (repositorioCategoria.ExisteTitulo(dto.Titulo, dto.Id))
            return Falha(nameof(dto.Titulo), "Ja existe uma categoria com este titulo.");

        Categoria categoria = new Categoria(dto.Titulo);

        Result resultadoValidacao = ValidarEntidade(categoria);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        bool conseguiuEditar = repositorioCategoria.Editar(dto.Id, categoria);

        if (!conseguiuEditar)
            return Result.Fail("Categoria nao encontrada.");

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        if (repositorioCategoria.PossuiDespesasVinculadas(id))
            return Result.Fail("Nao e possivel excluir uma categoria vinculada a despesas.");

        bool conseguiuExcluir = repositorioCategoria.Excluir(id);

        if (!conseguiuExcluir)
            return Result.Fail("Categoria nao encontrada.");

        return Result.Ok();
    }

    public List<ListarCategoriasDto> SelecionarTodos()
    {
        return repositorioCategoria
            .SelecionarTodos()
            .Select(c => new ListarCategoriasDto(c.Id, c.Titulo))
            .ToList();
    }

    public Result<DetalhesCategoriaDto> SelecionarPorId(Guid id)
    {
        Categoria? categoria = repositorioCategoria.SelecionarPorId(id);

        if (categoria == null)
            return Result.Fail("Categoria nao encontrada.");

        return Result.Ok(new DetalhesCategoriaDto(categoria.Id, categoria.Titulo));
    }

    private static Result ValidarEntidade(Categoria categoria)
    {
        List<string> erros = categoria.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        return Result.Fail(new Error(erros.First()).WithMetadata("Campo", string.Empty));
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }
}