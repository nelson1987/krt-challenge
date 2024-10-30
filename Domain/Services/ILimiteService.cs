using Domain.Entities;

namespace Domain.Services;

public interface ILimiteService
{
    Task<Limite> CreateAsync(Limite limite, CancellationToken cancellationToken = default);

    Task<Limite> UpdateAsync(Limite limite, CancellationToken cancellationToken = default);

    Task DeleteAsync(string documento, string conta, CancellationToken cancellationToken = default);
}