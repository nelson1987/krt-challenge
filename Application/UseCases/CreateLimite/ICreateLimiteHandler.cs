using Application.UseCases.ChangeLimite;
using FluentResults;

namespace Application.UseCases.CreateLimite;

public interface ICreateLimiteHandler
{
    Task<Result<CreateLimiteResponse>> HandleAsync(CreateLimiteCommand command, CancellationToken cancellationToken = default);
}