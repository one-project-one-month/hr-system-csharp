namespace HRSystem.Csharp.Domain.Helpers;

public class Generator
{
    public string GenerateProjectCode(string? lastProjectCode)
    {
        if (lastProjectCode is null) return "PJ0001";

        int lastNumber = int.Parse(lastProjectCode.Substring(2));

        return "PJ" + (lastNumber + 1).ToString("D4");
    }
}