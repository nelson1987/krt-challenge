using Domain.DTOs;
using Domain.Entities;

namespace Domain.Helpers;

public static class DtoBuilderExtensions
{
    public static LimiteDto ToDto(this Limite limite)
    {
        return new LimiteDto
        {
            Document = limite.Documento,
            Branch = limite.Agencia,
            Account = limite.Conta,
            Value = limite.Valor
        };
    }
}