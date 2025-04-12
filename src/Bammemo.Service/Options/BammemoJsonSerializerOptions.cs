using System.Text.Json;

namespace Bammemo.Service.Options;

public class BammemoJsonSerializerOptions
{
    private static readonly JsonSerializerOptions _camelCaseOption = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public static JsonSerializerOptions CamelCaseOption => _camelCaseOption;
}
