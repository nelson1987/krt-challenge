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
    public async Task IncluirLimite_DadosValidos_RetornaDadosInseridos()
        => _sut
            .TestValidate(_command)
            .ShouldNotHaveAnyValidationErrors();

    [Fact]
    public async Task IncluirLimite_DocumentoValorDefault_RetornaFalha()
        => _sut
            .TestValidate(_command with { Documento = default })
            .ShouldHaveValidationErrorFor(x => x.Documento)
            .Only();

    [Fact]
    public async Task IncluirLimite_DocumentoVazio_RetornaFalha()
        => _sut
            .TestValidate(_command with { Documento = "" })
            .ShouldHaveValidationErrorFor(x => x.Documento)
            .Only();

    [Fact]
    public async Task IncluirLimite_DocumentoEspacoEmBranco_RetornaFalha()
        => _sut
            .TestValidate(_command with { Documento = " " })
            .ShouldHaveValidationErrorFor(x => x.Documento)
            .Only();

    [Fact]
    public async Task IncluirLimite_DocumentoNulo_RetornaFalha()
        => _sut
            .TestValidate(_command with { Documento = null })
            .ShouldHaveValidationErrorFor(x => x.Documento)
            .Only();

    [Fact]
    public async Task IncluirLimite_AgenciaValorDefault_RetornaFalha()
        => _sut
            .TestValidate(_command with { Agencia = default })
            .ShouldHaveValidationErrorFor(x => x.Agencia)
            .Only();

    [Fact]
    public async Task IncluirLimite_AgenciaVazio_RetornaFalha()
        => _sut
            .TestValidate(_command with { Agencia = "" })
            .ShouldHaveValidationErrorFor(x => x.Agencia)
            .Only();

    [Fact]
    public async Task IncluirLimite_AgenciaEspacoEmBranco_RetornaFalha()
        => _sut
            .TestValidate(_command with { Agencia = " " })
            .ShouldHaveValidationErrorFor(x => x.Agencia)
            .Only();

    [Fact]
    public async Task IncluirLimite_AgenciaNulo_RetornaFalha()
        => _sut
            .TestValidate(_command with { Agencia = null })
            .ShouldHaveValidationErrorFor(x => x.Agencia)
            .Only();

    [Fact]
    public async Task IncluirLimite_ContaValorDefault_RetornaFalha()
        => _sut
            .TestValidate(_command with { Conta = default })
            .ShouldHaveValidationErrorFor(x => x.Conta)
            .Only();

    [Fact]
    public async Task IncluirLimite_ContaVazio_RetornaFalha()
        => _sut
            .TestValidate(_command with { Conta = "" })
            .ShouldHaveValidationErrorFor(x => x.Conta)
            .Only();

    [Fact]
    public async Task IncluirLimite_ContaEspacoEmBranco_RetornaFalha()
        => _sut
            .TestValidate(_command with { Conta = " " })
            .ShouldHaveValidationErrorFor(x => x.Conta)
            .Only();

    [Fact]
    public async Task IncluirLimite_ContaNulo_RetornaFalha()
        => _sut
            .TestValidate(_command with { Conta = null })
            .ShouldHaveValidationErrorFor(x => x.Conta)
            .Only();

    [Fact]
    public async Task IncluirLimite_ValorIgualAZero_RetornaFalha()
        => _sut
            .TestValidate(_command with { Valor = 0.00M })
            .ShouldHaveValidationErrorFor(x => x.Valor)
            .Only();

    [Fact]
    public async Task IncluirLimite_ValorMenorQueZero_RetornaFalha()
        => _sut
            .TestValidate(_command with { Valor = -0.01M })
            .ShouldHaveValidationErrorFor(x => x.Valor)
            .Only();
}

public class CreateLimiteHandlerUnitTests : UnitTestsBase
{
    private readonly CreateLimiteHandler _sut;
    private readonly CreateLimiteCommand _command;

    public CreateLimiteHandlerUnitTests()
    {
        _command = _fixture.Create<CreateLimiteCommand>();
        _sut = _fixture.Create<CreateLimiteHandler>();
    }

    [Fact]
    public async Task IncluirLimite_DadosValidos_RetornaDadosInseridos()
    {
        var response = await _sut.Handle(_command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal(_command.Documento, response.Documento);
        Assert.Equal(_command.Agencia, response.Agencia);
        Assert.Equal(_command.Conta, response.Conta);
        Assert.Equal(_command.Valor, response.Valor);
    }
}