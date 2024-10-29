using Domain.Entities;

namespace Domain.Services;

public interface ILimiteService
{
    Task<Limite> Create(Limite limite, CancellationToken cancellationToken = default);
}