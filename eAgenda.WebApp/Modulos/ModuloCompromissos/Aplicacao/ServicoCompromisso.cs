using FluentResults;
using eAgenda.WebApp.Modulos.ModuloCompromissos.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloCompromissos.Aplicacao;

public class ServicoCompromisso
{
    private readonly IRepositorioCompromisso repositorioCompromisso;

    public ServicoCompromisso(IRepositorioCompromisso repositorioCompromisso)
    {
        this.repositorioCompromisso = repositorioCompromisso;
    }

    public Result Cadastrar(CadastrarCompromissoDto dto)
    {
        if (repositorioCompromisso.ExisteConflitoDeHorario(dto.DataOcorrencia, dto.HoraInicio, dto.HoraTermino))
            return Result.Fail("Ja existe um compromisso neste periodo.");

        Compromisso compromisso = new(dto.Assunto, dto.DataOcorrencia, dto.HoraInicio, dto.HoraTermino, dto.Tipo, dto.Local, dto.Link, dto.ContatoId);

        Result resultadoValidacao = ValidarEntidade(compromisso);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioCompromisso.Cadastrar(compromisso);

        return Result.Ok();
    }

    public Result Editar(EditarCompromissoDto dto)
    {
        if (repositorioCompromisso.ExisteConflitoDeHorario(dto.DataOcorrencia, dto.HoraInicio, dto.HoraTermino, dto.Id))
            return Result.Fail("Ja existe um compromisso neste periodo.");

        Compromisso compromisso = new(dto.Assunto, dto.DataOcorrencia, dto.HoraInicio, dto.HoraTermino, dto.Tipo, dto.Local, dto.Link, dto.ContatoId);

        Result resultadoValidacao = ValidarEntidade(compromisso);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        bool conseguiuEditar = repositorioCompromisso.Editar(dto.Id, compromisso);

        if (!conseguiuEditar)
            return Result.Fail("Compromisso nao encontrado.");

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        bool conseguiuExcluir = repositorioCompromisso.Excluir(id);

        if (!conseguiuExcluir)
            return Result.Fail("Compromisso nao encontrado.");

        return Result.Ok();
    }

    public List<ListarCompromissosDto> SelecionarTodos()
    {
        return repositorioCompromisso.SelecionarTodos()
            .Select(c => new ListarCompromissosDto(c.Id, c.Assunto, c.DataOcorrencia, c.HoraInicio, c.HoraTermino, c.Tipo))
            .ToList();
    }

    public Result<DetalhesCompromissoDto> SelecionarPorId(Guid id)
    {
        Compromisso? c = repositorioCompromisso.SelecionarPorId(id);

        if (c == null)
            return Result.Fail("Compromisso nao encontrado.");

        return Result.Ok(new DetalhesCompromissoDto(c.Id, c.Assunto, c.DataOcorrencia, c.HoraInicio, c.HoraTermino, c.Tipo, c.Local, c.Link, c.ContatoId));
    }

    private static Result ValidarEntidade(Compromisso compromisso)
    {
        List<string> erros = compromisso.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        return Result.Fail(new Error(erros.First()).WithMetadata("Campo", string.Empty));
    }
}