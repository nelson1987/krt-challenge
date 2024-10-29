using System.Text.Json.Serialization;

namespace Domain.DTOs;

public class LimiteDto
{
    [JsonPropertyName("document")]
    public string Document { get; init; } = default!;

    [JsonPropertyName("account")]
    public string Account { get; init; } = default!;

    [JsonPropertyName("branch")]
    public string Branch { get; init; } = default!;

    [JsonPropertyName("Value")]
    public decimal Value { get; init; }
}