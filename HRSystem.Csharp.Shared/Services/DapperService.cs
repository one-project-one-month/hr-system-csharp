using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;

namespace HRSystem.Csharp.Shared.Services;

public class DapperService
{
    private readonly IDbConnection _dbConnection;
    private readonly ILogger<DapperService> _logger;

    public DapperService(IDbConnection dbConnection, ILogger<DapperService> logger)
    {
        _dbConnection = dbConnection;
        _logger = logger;
    }

    public async Task<T> QueryStoredProcedureWithOutput<T>(string spName, DynamicParameters parameters)
    {
        try
        {
            if (_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            using var multi = await _dbConnection.QueryMultipleAsync(
                spName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
                
            // Read the single object from the first result set.
            // The output parameters are populated in the 'parameters' object after this call.
            var result = await multi.ReadSingleOrDefaultAsync<T>();

            return result!;
        }
        catch (Exception ex)
        {
            // Log the custom error and return a default value.
            _logger.LogError(ex.ToString());
            return default!;
        }
    }

    public async Task<IEnumerable<T>> QueryStoredProcedureWithMultipleResults<T>(string spName, DynamicParameters parameters)
    {
        try
        {
            if (_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            using var multi = await _dbConnection.QueryMultipleAsync(
                spName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
            var results = await multi.ReadAsync<T>();
            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return Enumerable.Empty<T>();
        }
    }
    
    public async Task<int> ExecuteAsync(
        string query,
        object parameters = null,
        CommandType commandType = CommandType.Text)
    {
        var result = await _dbConnection.ExecuteAsync(
            query,
            parameters,
            commandType: commandType);
        return result;
    }
    
    public async Task<List<T>> GetListAsync<T>(string query, object parameters = null,
        CommandType commandType = CommandType.Text)
    {
        var result = await _dbConnection.QueryAsync<T>(
            query,
            parameters,
            commandType: commandType
        );
        return result.ToList();
    }
}