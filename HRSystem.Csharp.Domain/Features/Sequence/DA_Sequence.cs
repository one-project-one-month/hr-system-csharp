using System.Data;
using Dapper;
using HRSystem.Csharp.Shared.Services;

namespace HRSystem.Csharp.Domain.Features.Sequence;

public class DA_Sequence
{
    private readonly DapperService _dapperService;

    public DA_Sequence(DapperService dapperService)
    {
        _dapperService = dapperService;
    }

    public async Task<string> GenerateCodeAsync(string codePrefix)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@CodePrefix", codePrefix, DbType.String, ParameterDirection.Input);
        parameters.Add("@GeneratedCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

        await _dapperService.QueryStoredProcedureWithOutput<object>("sp_GenerateCode", parameters);

        var generatedCode = parameters.Get<string>("@GeneratedCode");
        return generatedCode;
    }
}