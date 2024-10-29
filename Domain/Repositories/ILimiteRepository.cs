using Domain.DTOs;

namespace Domain.Repositories;

public interface ILimiteRepository
{
    Task<LimiteDto> InsertAsync(LimiteDto limite, CancellationToken cancellationToken = default);

    Task<LimiteDto?> GetAsync(string documento, string agencia, string conta, CancellationToken cancellationToken = default);

    Task DeleteAsync(string documento, string conta, CancellationToken cancellationToken = default);

    Task<LimiteDto> UpdateAsync(LimiteDto limite, CancellationToken cancellationToken = default);
}