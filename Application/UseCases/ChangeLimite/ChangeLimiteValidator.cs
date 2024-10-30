using FluentValidation;

namespace Application.UseCases.ChangeLimite;

public class ChangeLimiteValidator : AbstractValidator<ChangeLimiteCommand>
{
    public ChangeLimiteValidator()
    {
        RuleFor(x => x.Documento)
            .NotEmpty();
        RuleFor(x => x.Agencia)
            .NotEmpty();
        RuleFor(x => x.Conta)
            .NotEmpty();
        RuleFor(x => x.Valor)
            .GreaterThan(0);
    }
}