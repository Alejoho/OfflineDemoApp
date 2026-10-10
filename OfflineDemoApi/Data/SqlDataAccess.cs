using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace OfflineDemoApi.Data;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public async Task<List<T>> LoadData<T, U>(string storedProcedure,
                                              U parameters,
                                              string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName) ??
            throw new KeyNotFoundException("Did not find the connection string specified");

        using IDbConnection connection = new SqlConnection(connectionString);

        List<T> output = (await connection.QueryAsync<T>(
            storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure)).ToList();

        return output;
    }

    public async Task<T> SaveDataScalar<T, U>(string storedProcedure,
                                              U parameters,
                                              string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName) ??
            throw new KeyNotFoundException("Did not find the connection string specified");

        using IDbConnection connection = new SqlConnection(connectionString);

        T? output = await connection.ExecuteScalarAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

        return output ?? throw new ArgumentNullException("The return value was null, which is an invalid result.");
    }

    public async Task SaveData<T>(string storedProcedure,
                                  T parameters,
                                  string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName) ??
            throw new KeyNotFoundException("Did not find the connection string specified");

        using IDbConnection connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }
}
