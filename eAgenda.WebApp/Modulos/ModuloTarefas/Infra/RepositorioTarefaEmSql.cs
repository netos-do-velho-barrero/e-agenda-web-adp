using Dapper;
using eAgenda.WebApp.Compartilhado.Infra.Sql;
using eAgenda.WebApp.Modulos.ModuloTarefas.Dominio;
using Microsoft.Data.SqlClient;

namespace eAgenda.WebApp.Modulos.ModuloTarefas.Infra;

public sealed class RepositorioTarefaEmSql(ISqlConnectionFactory connectionFactory) : IRepositorioTarefa
{
    public void Cadastrar(Tarefa entidade)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        using SqlTransaction transacao = conexao.BeginTransaction();

        const string tarefaSql = """
            INSERT INTO dbo.TBTarefa
            (Id, Titulo, Prioridade, DataCriacao, DataConclusao, Concluida, PercentualConcluido)
            VALUES
            (@Id, @Titulo, @Prioridade, @DataCriacao, @DataConclusao, @Concluida, @PercentualConcluido);
        """;

        conexao.Execute(tarefaSql, entidade, transacao);

        InserirItens(conexao, transacao, entidade.Id, entidade.Itens);

        transacao.Commit();
    }

    public bool Editar(Guid idSelecionado, Tarefa entidadeAtualizada)
    {
        entidadeAtualizada.Id = idSelecionado;

        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        using SqlTransaction transacao = conexao.BeginTransaction();

        const string tarefaSql = """
            UPDATE dbo.TBTarefa
            SET Titulo = @Titulo,
                Prioridade = @Prioridade,
                DataConclusao = @DataConclusao,
                Concluida = @Concluida,
                PercentualConcluido = @PercentualConcluido
            WHERE Id = @Id;
        """;

        bool conseguiuEditar = conexao.Execute(tarefaSql, entidadeAtualizada, transacao) == 1;

        if (!conseguiuEditar)
        {
            transacao.Rollback();
            return false;
        }

        conexao.Execute(
            "DELETE FROM dbo.TBItemTarefa WHERE TarefaId = @TarefaId;",
            new { TarefaId = idSelecionado },
            transacao
        );

        InserirItens(conexao, transacao, idSelecionado, entidadeAtualizada.Itens);

        transacao.Commit();

        return true;
    }

    public bool Excluir(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        using SqlTransaction transacao = conexao.BeginTransaction();

        conexao.Execute(
            "DELETE FROM dbo.TBItemTarefa WHERE TarefaId = @TarefaId;",
            new { TarefaId = idSelecionado },
            transacao
        );

        bool conseguiuExcluir = conexao.Execute(
            "DELETE FROM dbo.TBTarefa WHERE Id = @Id;",
            new { Id = idSelecionado },
            transacao
        ) == 1;

        transacao.Commit();

        return conseguiuExcluir;
    }

    public Tarefa? SelecionarPorId(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        const string sql = """
            SELECT
                Id,
                Titulo,
                Prioridade,
                DataCriacao,
                DataConclusao,
                Concluida,
                PercentualConcluido
            FROM dbo.TBTarefa
            WHERE Id = @Id;
        """;

        Tarefa? tarefa = conexao.QuerySingleOrDefault<Tarefa>(sql, new { Id = idSelecionado });

        if (tarefa == null)
            return null;

        tarefa.Itens = SelecionarItens(conexao, tarefa.Id);

        return tarefa;
    }

    public List<Tarefa> SelecionarTodos()
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        const string sql = """
            SELECT
                Id,
                Titulo,
                Prioridade,
                DataCriacao,
                DataConclusao,
                Concluida,
                PercentualConcluido
            FROM dbo.TBTarefa
            ORDER BY Prioridade DESC, DataCriacao DESC;
        """;

        List<Tarefa> tarefas = conexao.Query<Tarefa>(sql).ToList();

        foreach (Tarefa tarefa in tarefas)
            tarefa.Itens = SelecionarItens(conexao, tarefa.Id);

        return tarefas;
    }

    public List<Tarefa> Filtrar(Predicate<Tarefa> filtro)
    {
        return SelecionarTodos().FindAll(filtro);
    }

    public List<Tarefa> SelecionarPendentes()
    {
        return Filtrar(t => !t.Concluida);
    }

    public List<Tarefa> SelecionarConcluidas()
    {
        return Filtrar(t => t.Concluida);
    }

    public List<Tarefa> SelecionarPorPrioridade(PrioridadeTarefa prioridade)
    {
        return Filtrar(t => t.Prioridade == prioridade);
    }

    private static void InserirItens(
        SqlConnection conexao,
        SqlTransaction transacao,
        Guid tarefaId,
        List<ItemTarefa> itens
    )
    {
        const string sql = """
            INSERT INTO dbo.TBItemTarefa (Id, Titulo, Concluido, TarefaId)
            VALUES (@Id, @Titulo, @Concluido, @TarefaId);
        """;

        foreach (ItemTarefa item in itens)
        {
            conexao.Execute(sql, new
            {
                item.Id,
                item.Titulo,
                item.Concluido,
                TarefaId = tarefaId
            }, transacao);
        }
    }

    private static List<ItemTarefa> SelecionarItens(SqlConnection conexao, Guid tarefaId)
    {
        const string sql = """
            SELECT Id, Titulo, Concluido
            FROM dbo.TBItemTarefa
            WHERE TarefaId = @TarefaId
            ORDER BY Titulo;
        """;

        return conexao.Query<ItemTarefa>(sql, new { TarefaId = tarefaId }).ToList();
    }
}