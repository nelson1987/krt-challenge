using System.Text.Json;

namespace Domain.Helpers;

public static class LoggerJsonExtensions
{
    public static string ToJson(this object obj)
    {
        return JsonSerializer.Serialize(obj);
    }
}