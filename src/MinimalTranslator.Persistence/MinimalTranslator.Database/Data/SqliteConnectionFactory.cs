using Microsoft.Data.Sqlite;
using MinimalTranslator.Application.Abstractions.Data;
using System.Data;

namespace MinimalTranslator.Database.Data;

public class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateOpenConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();

        return connection;
    }
}
