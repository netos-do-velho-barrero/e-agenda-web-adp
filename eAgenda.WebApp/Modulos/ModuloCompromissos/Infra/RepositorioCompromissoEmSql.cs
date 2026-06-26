using Dapper;
using eAgenda.WebApp.Compartilhado.Infra.Sql;
using eAgenda.WebApp.Modulos.ModuloCompromissos.Dominio;
using Microsoft.Data.SqlClient;

namespace eAgenda.WebApp.Modulos.ModuloCompromissos.Infra;

public sealed class RepositorioCompromissoEmSql(ISqlConnectionFactory connectionFactory) : IRepositorioCompromisso
{
    public void Cadastrar(Compromisso entidade)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        const string sql = """
            INSERT INTO dbo.TBCompromisso
            (Id, Assunto, DataOcorrencia, HoraInicio, HoraTermino, TipoCompromisso, Local, Link, ContatoId)
            VALUES
            (@Id, @Assunto, @DataOcorrencia, @HoraInicio, @HoraTermino, @Tipo, @Local, @Link, @ContatoId);
        """;

        conexao.Execute(sql, entidade);
    }

    public bool Editar(Guid idSelecionado, Compromisso entidadeAtualizada)
    {
        entidadeAtualizada.Id = idSelecionado;

        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        const string sql = """
            UPDATE dbo.TBCompromisso
            SET Assunto = @Assunto,
                DataOcorrencia = @DataOcorrencia,
                HoraInicio = @HoraInicio,
                HoraTermino = @HoraTermino,
                TipoCompromisso = @Tipo,
                Local = @Local,
                Link = @Link,
                ContatoId = @ContatoId
            WHERE Id = @Id;
        """;

        return conexao.Execute(sql, entidadeAtualizada) == 1;
    }

    public bool Excluir(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        return conexao.Execute("DELETE FROM dbo.TBCompromisso WHERE Id = @Id;", new { Id = idSelecionado }) == 1;
    }

    public Compromisso? SelecionarPorId(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        const string sql = """
            SELECT Id, Assunto, DataOcorrencia, HoraInicio, HoraTermino,
                   TipoCompromisso AS Tipo, Local, Link, ContatoId
            FROM dbo.TBCompromisso
            WHERE Id = @Id;
        """;

        return conexao.QuerySingleOrDefault<Compromisso>(sql, new { Id = idSelecionado });
    }

    public List<Compromisso> SelecionarTodos()
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        const string sql = """
            SELECT Id, Assunto, DataOcorrencia, HoraInicio, HoraTermino,
                   TipoCompromisso AS Tipo, Local, Link, ContatoId
            FROM dbo.TBCompromisso
            ORDER BY DataOcorrencia, HoraInicio;
        """;

        return conexao.Query<Compromisso>(sql).ToList();
    }

    public List<Compromisso> Filtrar(Predicate<Compromisso> filtro)
    {
        return SelecionarTodos().FindAll(filtro);
    }

    public bool ExisteConflitoDeHorario(DateOnly data, TimeOnly horaInicio, TimeOnly horaTermino, Guid? idIgnorado = null)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        const string sql = """
            SELECT COUNT(1)
            FROM dbo.TBCompromisso
            WHERE DataOcorrencia = @Data
              AND HoraInicio < @HoraTermino
              AND HoraTermino > @HoraInicio
              AND (@IdIgnorado IS NULL OR Id <> @IdIgnorado);
        """;

        return conexao.ExecuteScalar<int>(sql, new { Data = data, HoraInicio = horaInicio, HoraTermino = horaTermino, IdIgnorado = idIgnorado }) > 0;
    }

    public List<Compromisso> SelecionarCompromissosFuturos()
    {
        DateOnly hoje = DateOnly.FromDateTime(DateTime.Today);

        return Filtrar(c => c.DataOcorrencia >= hoje);
    }

    public List<Compromisso> SelecionarCompromissosPassados()
    {
        DateOnly hoje = DateOnly.FromDateTime(DateTime.Today);

        return Filtrar(c => c.DataOcorrencia < hoje);
    }
}