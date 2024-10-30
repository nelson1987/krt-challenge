using FluentResults;

namespace Application.UseCases.ChangeLimite;

public interface IChangeLimiteHandler
{
    Task<Result<ChangeLimiteResponse>> HandleAsync(ChangeLimiteCommand command, CancellationToken cancellationToken = default);
}