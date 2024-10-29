using FluentValidation;

namespace Application.UseCases.CreateLimite;

public class CreateLimiteValidator : AbstractValidator<CreateLimiteCommand>
{
    public CreateLimiteValidator()
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

public class DeleteLimiteValidator : AbstractValidator<DeleteLimiteCommand>
{
    public DeleteLimiteValidator()
    {
        RuleFor(x => x.Documento)
            .NotEmpty();
        RuleFor(x => x.Conta)
            .NotEmpty();
    }
}