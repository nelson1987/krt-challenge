using Application.UseCases.ChangeLimite;
using Application.UseCases.CreateLimite;
using AutoFixture;
using FluentValidation.TestHelper;

namespace UnitTests;

public class CreateLimiteCommandUnitTests : UnitTestsBase
{
    private readonly CreateLimiteValidator _sut;
    private readonly CreateLimiteCommand _command;

    public CreateLimiteCommandUnitTests()
    {
        _command = _fixture.Create<CreateLimiteCommand>();
        _sut = _fixture.Create<CreateLimiteValidator>();
    }

    [Fact]
    public void IncluirLimite_DadosValidos_RetornaDadosInseridos()
        => _sut
            .TestValidate(_command)
            .ShouldNotHaveAnyValidationErrors();

    [Fact]
    public void IncluirLimite_DocumentoVazio_RetornaFalha()
        => _sut
            .TestValidate(_command with { Documento = "" })
            .ShouldHaveValidationErrorFor(x => x.Documento)
            .Only();

    [Fact]
    public void IncluirLimite_DocumentoEspacoEmBranco_RetornaFalha()
        => _sut
            .TestValidate(_command with { Documento = " " })
            .ShouldHaveValidationErrorFor(x => x.Documento)
            .Only();

    [Fact]
    public void IncluirLimite_AgenciaVazio_RetornaFalha()
        => _sut
            .TestValidate(_command with { Agencia = "" })
            .ShouldHaveValidationErrorFor(x => x.Agencia)
            .Only();

    [Fact]
    public void IncluirLimite_AgenciaEspacoEmBranco_RetornaFalha()
        => _sut
            .TestValidate(_command with { Agencia = " " })
            .ShouldHaveValidationErrorFor(x => x.Agencia)
            .Only();

    [Fact]
    public void IncluirLimite_ContaVazio_RetornaFalha()
        => _sut
            .TestValidate(_command with { Conta = "" })
            .ShouldHaveValidationErrorFor(x => x.Conta)
            .Only();

    [Fact]
    public void IncluirLimite_ContaEspacoEmBranco_RetornaFalha()
        => _sut
            .TestValidate(_command with { Conta = " " })
            .ShouldHaveValidationErrorFor(x => x.Conta)
            .Only();

    [Fact]
    public void IncluirLimite_ValorIgualAZero_RetornaFalha()
        => _sut
            .TestValidate(_command with { Valor = 0.00M })
            .ShouldHaveValidationErrorFor(x => x.Valor)
            .Only();

    [Fact]
    public void IncluirLimite_ValorMenorQueZero_RetornaFalha()
        => _sut
            .TestValidate(_command with { Valor = -0.01M })
            .ShouldHaveValidationErrorFor(x => x.Valor)
            .Only();
}