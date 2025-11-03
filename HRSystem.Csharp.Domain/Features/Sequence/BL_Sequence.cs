using System.Data;
using Dapper;

namespace HRSystem.Csharp.Domain.Features.Sequence;

public class BL_Sequence
{
    private readonly DA_Sequence _daSequence;

    public BL_Sequence(DA_Sequence daSequence)
    {
        _daSequence = daSequence;
    }

    public async Task<string> GenerateCodeAsync(string codePrefix)
    {
        var generatedCode = await _daSequence.GenerateCodeAsync(codePrefix);
        return generatedCode;
    }

}