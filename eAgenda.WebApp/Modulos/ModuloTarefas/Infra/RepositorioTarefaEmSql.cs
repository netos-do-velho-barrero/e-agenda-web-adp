using System.Data;
using Dapper;
using eAgenda.WebApp.Compartilhado.Infra.Sql;
using eAgenda.WebApp.Modulos.ModuloTarefas.Dominio;
using Microsoft.Data.SqlClient;

namespace eAgenda.WebApp.Modulos.ModuloTarefas.Infra;

public sealed class RepositorioTarefaEmSql(ISqlConnectionFactory connectionFactory) : IRepositorioTarefa
{
    static RepositorioTarefaEmSql()
    {
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new NullableDateOnlyTypeHandler());
    }

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

        conexao.Execute(tarefaSql, CriarParametros(entidade), transacao);

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

        var parametros = new
        {
            entidadeAtualizada.Id,
            entidadeAtualizada.Titulo,
            entidadeAtualizada.Prioridade,
            DataConclusao = entidadeAtualizada.DataConclusao.HasValue
                ? entidadeAtualizada.DataConclusao.Value.ToDateTime(TimeOnly.MinValue)
                : (DateTime?)null,
            entidadeAtualizada.Concluida,
            entidadeAtualizada.PercentualConcluido
        };

        bool conseguiuEditar = conexao.Execute(tarefaSql, parametros, transacao) == 1;

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

    private static object CriarParametros(Tarefa entidade)
    {
        return new
        {
            entidade.Id,
            entidade.Titulo,
            entidade.Prioridade,
            DataCriacao = entidade.DataCriacao.ToDateTime(TimeOnly.MinValue),
            DataConclusao = entidade.DataConclusao.HasValue
                ? entidade.DataConclusao.Value.ToDateTime(TimeOnly.MinValue)
                : (DateTime?)null,
            entidade.Concluida,
            entidade.PercentualConcluido
        };
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

    private sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override DateOnly Parse(object value)
        {
            return value switch
            {
                DateTime dt => DateOnly.FromDateTime(dt),
                DateOnly d => d,
                _ => DateOnly.Parse(value.ToString()!)
            };
        }

        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            parameter.DbType = DbType.Date;
            parameter.Value = value.ToDateTime(TimeOnly.MinValue);
        }
    }

    private sealed class NullableDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly?>
    {
        public override DateOnly? Parse(object value)
        {
            return value switch
            {
                null => null,
                DateTime dt => DateOnly.FromDateTime(dt),
                DateOnly d => d,
                _ => DateOnly.Parse(value.ToString()!)
            };
        }

        public override void SetValue(IDbDataParameter parameter, DateOnly? value)
        {
            parameter.DbType = DbType.Date;
            parameter.Value = value.HasValue
                ? value.Value.ToDateTime(TimeOnly.MinValue)
                : DBNull.Value;
        }
    }
}