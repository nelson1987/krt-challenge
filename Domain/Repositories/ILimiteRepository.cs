using Domain.DTOs;

namespace Domain.Repositories;

public interface ILimiteRepository
{
    Task<LimiteDto> Incluir(LimiteDto limite, CancellationToken cancellationToken = default);

    Task<LimiteDto?> Buscar(string documento, string agencia, string conta, CancellationToken cancellationToken = default);
}