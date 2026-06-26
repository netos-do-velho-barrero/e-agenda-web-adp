using FluentResults;
using eAgenda.WebApp.Modulos.ModuloTarefas.Dominio;

namespace eAgenda.WebApp.Modulos.ModuloTarefas.Aplicacao;

public class ServicoTarefa
{
    private readonly IRepositorioTarefa repositorioTarefa;

    public ServicoTarefa(IRepositorioTarefa repositorioTarefa)
    {
        this.repositorioTarefa = repositorioTarefa;
    }

    public Result Cadastrar(CadastrarTarefaDto dto)
    {
        Tarefa tarefa = new(dto.Titulo, dto.Prioridade)
        {
            Itens = dto.Itens.Select(i => new ItemTarefa(i)).ToList()
        };

        tarefa.AtualizarPercentual();

        Result resultadoValidacao = ValidarEntidade(tarefa);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioTarefa.Cadastrar(tarefa);

        return Result.Ok();
    }

    public Result Editar(EditarTarefaDto dto)
    {
        Tarefa tarefa = new(dto.Titulo, dto.Prioridade)
        {
            Itens = dto.Itens.Select(i => new ItemTarefa(i.Titulo)
            {
                Id = i.Id,
                Concluido = i.Concluido
            }).ToList()
        };

        tarefa.AtualizarPercentual();

        Result resultadoValidacao = ValidarEntidade(tarefa);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        bool conseguiuEditar = repositorioTarefa.Editar(dto.Id, tarefa);

        if (!conseguiuEditar)
            return Result.Fail("Tarefa nao encontrada.");

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        bool conseguiuExcluir = repositorioTarefa.Excluir(id);

        if (!conseguiuExcluir)
            return Result.Fail("Tarefa nao encontrada.");

        return Result.Ok();
    }

    public List<ListarTarefasDto> SelecionarTodos()
    {
        return repositorioTarefa.SelecionarTodos()
            .Select(t => new ListarTarefasDto(t.Id, t.Titulo, t.Prioridade, t.Concluida, t.PercentualConcluido))
            .ToList();
    }

    public Result<DetalhesTarefaDto> SelecionarPorId(Guid id)
    {
        Tarefa? tarefa = repositorioTarefa.SelecionarPorId(id);

        if (tarefa == null)
            return Result.Fail("Tarefa nao encontrada.");

        List<ItemTarefaDto> itens = tarefa.Itens
            .Select(i => new ItemTarefaDto(i.Id, i.Titulo, i.Concluido))
            .ToList();

        return Result.Ok(new DetalhesTarefaDto(tarefa.Id, tarefa.Titulo, tarefa.Prioridade, tarefa.Concluida, tarefa.PercentualConcluido, itens));
    }

    private static Result ValidarEntidade(Tarefa tarefa)
    {
        List<string> erros = tarefa.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        return Result.Fail(new Error(erros.First()).WithMetadata("Campo", string.Empty));
    }
}