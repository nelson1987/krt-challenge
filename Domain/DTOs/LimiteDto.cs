using Domain.Helpers;
using System.Text.Json.Serialization;

namespace Domain.DTOs;

public record LimiteDto
{
    [JsonPropertyName(Mensagem.PROPRIEDADE_DOCUMENT)]
    public string Document { get; init; } = default!;

    [JsonPropertyName(Mensagem.PROPRIEDADE_ACCOUNT)]
    public string Account { get; init; } = default!;

    [JsonPropertyName(Mensagem.PROPRIEDADE_BRANCH)]
    public string Branch { get; init; } = default!;

    [JsonPropertyName(Mensagem.PROPRIEDADE_VALUE)]
    public decimal Value { get; init; }
}