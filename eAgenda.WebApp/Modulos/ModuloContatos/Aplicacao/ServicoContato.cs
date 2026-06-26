using FluentResults;
using eAgenda.WebApp.Modulos.ModuloContatos.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloContatos.Aplicacao;

public class ServicoContato
{
    private readonly IRepositorioContato repositorioContato;

    public ServicoContato(IRepositorioContato repositorioContato)
    {
        this.repositorioContato = repositorioContato;
    }

    public Result Cadastrar(CadastrarContatoDto dto)
    {
        if (repositorioContato.ExisteEmail(dto.Email))
            return Falha(nameof(dto.Email), "Ja existe um contato com este email.");

        if (repositorioContato.ExisteTelefone(dto.Telefone))
            return Falha(nameof(dto.Telefone), "Ja existe um contato com este telefone.");

        Contato contato = new(dto.Nome, dto.Email, dto.Telefone, dto.Cargo, dto.Empresa);

        Result resultadoValidacao = ValidarEntidade(contato);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioContato.Cadastrar(contato);

        return Result.Ok();
    }

    public Result Editar(EditarContatoDto dto)
    {
        if (repositorioContato.ExisteEmail(dto.Email, dto.Id))
            return Falha(nameof(dto.Email), "Ja existe um contato com este email.");

        if (repositorioContato.ExisteTelefone(dto.Telefone, dto.Id))
            return Falha(nameof(dto.Telefone), "Ja existe um contato com este telefone.");

        Contato contato = new(dto.Nome, dto.Email, dto.Telefone, dto.Cargo, dto.Empresa);

        Result resultadoValidacao = ValidarEntidade(contato);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        bool conseguiuEditar = repositorioContato.Editar(dto.Id, contato);

        if (!conseguiuEditar)
            return Result.Fail("Contato nao encontrado.");

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        if (repositorioContato.PossuiCompromissosVinculados(id))
            return Result.Fail("Nao e possivel excluir um contato vinculado a compromissos.");

        bool conseguiuExcluir = repositorioContato.Excluir(id);

        if (!conseguiuExcluir)
            return Result.Fail("Contato nao encontrado.");

        return Result.Ok();
    }

    public List<ListarContatosDto> SelecionarTodos()
    {
        return repositorioContato
            .SelecionarTodos()
            .Select(c => new ListarContatosDto(c.Id, c.Nome, c.Email, c.Telefone))
            .ToList();
    }

    public Result<DetalhesContatoDto> SelecionarPorId(Guid id)
    {
        Contato? contato = repositorioContato.SelecionarPorId(id);

        if (contato == null)
            return Result.Fail("Contato nao encontrado.");

        return Result.Ok(new DetalhesContatoDto(
            contato.Id,
            contato.Nome,
            contato.Email,
            contato.Telefone,
            contato.Cargo,
            contato.Empresa
        ));
    }

    private static Result ValidarEntidade(Contato contato)
    {
        List<string> erros = contato.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        return Result.Fail(new Error(erros.First()).WithMetadata("Campo", string.Empty));
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }
}