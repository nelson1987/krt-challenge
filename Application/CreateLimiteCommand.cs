namespace Application;

public record CreateLimiteCommand();
public record CreateLimiteResponse();

public class CreateLimiteHandler
{
    public async Task<CreateLimiteResponse> Create(CreateLimiteCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}