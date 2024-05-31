using System.Data;

namespace MinimalTranslator.Application.Abstractions.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateOpenConnection();
}