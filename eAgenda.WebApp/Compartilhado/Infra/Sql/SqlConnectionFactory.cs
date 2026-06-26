using Microsoft.Data.SqlClient;

namespace eAgenda.WebApp.Compartilhado.Infra.Sql;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}

public sealed class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
{
    private const string NomeConnectionString = "eAgendaDb";

    public SqlConnection CreateConnection()
    {
        string? connectionString = configuration.GetConnectionString(NomeConnectionString);

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException($"A connection string {NomeConnectionString} nao foi encontrada.");

        return new SqlConnection(connectionString);
    }
}