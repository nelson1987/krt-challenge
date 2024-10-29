using System.Text.Json.Serialization;

namespace Infrastructure.Repositories;

public class LimiteDto
{
    [JsonPropertyName("pk")]
    public string Pk => Id;

    [JsonPropertyName("sk")]
    public string Sk => Pk;

    [JsonPropertyName("id")]
    public string Id { get; init; } = default!;

    [JsonPropertyName("document")]
    public string Document { get; init; } = default!;

    [JsonPropertyName("account")]
    public string Account { get; init; } = default!;

    [JsonPropertyName("branch")]
    public string Branch { get; init; } = default!;

    [JsonPropertyName("valor")]
    public decimal Value { get; init; }
}
