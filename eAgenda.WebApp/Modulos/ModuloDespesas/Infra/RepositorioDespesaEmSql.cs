using Dapper;
using eAgenda.WebApp.Compartilhado.Infra.Sql;
using eAgenda.WebApp.Modulos.ModuloDespesas.Dominio;
using Microsoft.Data.SqlClient;

namespace eAgenda.WebApp.Modulos.ModuloDespesas.Infra;

public sealed class RepositorioDespesaEmSql(ISqlConnectionFactory connectionFactory) : IRepositorioDespesa
{
    public void Cadastrar(Despesa entidade)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        using SqlTransaction transacao = conexao.BeginTransaction();

        const string inserirDespesaSql = """
            INSERT INTO dbo.TBDespesa (Id, Descricao, DataOcorrencia, Valor, FormaPagamento)
            VALUES (@Id, @Descricao, @DataOcorrencia, @Valor, @FormaPagamento);
        """;

        conexao.Execute(inserirDespesaSql, entidade, transacao);
        InserirCategorias(conexao, transacao, entidade.Id, entidade.CategoriaIds);

        transacao.Commit();
    }

    public bool Editar(Guid idSelecionado, Despesa entidadeAtualizada)
    {
        entidadeAtualizada.Id = idSelecionado;

        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        using SqlTransaction transacao = conexao.BeginTransaction();

        const string atualizarDespesaSql = """
            UPDATE dbo.TBDespesa
            SET Descricao = @Descricao,
                DataOcorrencia = @DataOcorrencia,
                Valor = @Valor,
                FormaPagamento = @FormaPagamento
            WHERE Id = @Id;
        """;

        bool conseguiuEditar = conexao.Execute(atualizarDespesaSql, entidadeAtualizada, transacao) == 1;

        if (!conseguiuEditar)
        {
            transacao.Rollback();
            return false;
        }

        
        conexao.Execute("DELETE FROM TBDESPESA_TBCATEGORIA WHERE Despesa_Id = @Despesa_Id;", new { Despesa_Id = idSelecionado }, transacao);
        InserirCategorias(conexao, transacao, idSelecionado, entidadeAtualizada.CategoriaIds);

        transacao.Commit();

        return true;
    }

    public bool Excluir(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        using SqlTransaction transacao = conexao.BeginTransaction();

        // Corrigido para a tabela e colunas corretas
        conexao.Execute("DELETE FROM TBDESPESA_TBCATEGORIA WHERE Despesa_Id = @Despesa_Id;", new { Despesa_Id = idSelecionado }, transacao);

        bool conseguiuExcluir = conexao.Execute("DELETE FROM dbo.TBDespesa WHERE Id = @Id;", new { Id = idSelecionado }, transacao) == 1;

        transacao.Commit();

        return conseguiuExcluir;
    }

    public Despesa? SelecionarPorId(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        const string despesaSql = """
            SELECT Id, Descricao, DataOcorrencia, Valor, FormaPagamento
            FROM dbo.TBDespesa
            WHERE Id = @Id;
        """;

        Despesa? despesa = conexao.QuerySingleOrDefault<Despesa>(despesaSql, new { Id = idSelecionado });

        if (despesa == null)
            return null;

        despesa.CategoriaIds = SelecionarCategoriaIds(conexao, despesa.Id);

        return despesa;
    }

    public List<Despesa> SelecionarTodos()
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        const string sql = """
            SELECT Id, Descricao, DataOcorrencia, Valor, FormaPagamento
            FROM dbo.TBDespesa
            ORDER BY DataOcorrencia DESC;
        """;

        List<Despesa> despesas = conexao.Query<Despesa>(sql).ToList();

        foreach (Despesa despesa in despesas)
            despesa.CategoriaIds = SelecionarCategoriaIds(conexao, despesa.Id);

        return despesas;
    }

    public List<Despesa> Filtrar(Predicate<Despesa> filtro)
    {
        return SelecionarTodos().FindAll(filtro);
    }

    public List<Despesa> SelecionarPorCategoria(Guid categoriaId)
    {
        return Filtrar(d => d.CategoriaIds.Contains(categoriaId));
    }

    private static void InserirCategorias(SqlConnection conexao, SqlTransaction transacao, Guid despesaId, List<Guid> categoriaIds)
    {
      
        const string sql = """
            INSERT INTO TBDESPESA_TBCATEGORIA (Despesa_Id, Categoria_Id)
            VALUES (@Despesa_Id, @Categoria_Id);
        """;

        foreach (Guid categoriaId in categoriaIds)
            conexao.Execute(sql, new { Despesa_Id = despesaId, Categoria_Id = categoriaId }, transacao);
    }

    private static List<Guid> SelecionarCategoriaIds(SqlConnection conexao, Guid despesaId)
    {
      
        const string sql = """
            SELECT Categoria_Id
            FROM TBDESPESA_TBCATEGORIA
            WHERE Despesa_Id = @Despesa_Id;
        """;

        return conexao.Query<Guid>(sql, new { Despesa_Id = despesaId }).ToList();
    }
}