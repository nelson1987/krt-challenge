using Application.UseCases.ChangeLimite;
using Application.UseCases.CreateLimite;
using AutoFixture;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;

namespace UnitTests;

public class LimiteControllerUnitTests : UnitTestsBase
{
    private readonly LimiteController _sut;
    private readonly CreateLimiteCommand _command;
    private readonly Mock<IValidator<CreateLimiteCommand>> _validator;
    private readonly Mock<ICreateLimiteHandler> _handler;

    public LimiteControllerUnitTests()
    {
        _validator = new Mock<IValidator<CreateLimiteCommand>>();
        _handler = new Mock<ICreateLimiteHandler>();
        _command = new CreateLimiteCommand("Documento", "Agencia", "Conta", 0.01M);
        _sut = _fixture.Build<LimiteController>()
            .OmitAutoProperties()
            .Create();
    }

    [Fact]
    public async Task Post_QuandoDadosValidos_RetornaOk()
    {
        _validator
            .Setup(v => v.ValidateAsync(_command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _handler
            .Setup(h => h.HandleAsync(_command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<CreateLimiteResponse>()
                .WithValue(new CreateLimiteResponse(_command.Documento, _command.Agencia, _command.Conta, _command.Valor)));

        var postResult = await _sut.Post(_command, _validator.Object, _handler.Object, CancellationToken.None);

        ObjectResult result = Assert.IsType<ObjectResult>(postResult);
        Assert.Equal(201, result.StatusCode);
        CreateLimiteResponse response = Assert.IsType<CreateLimiteResponse>(result.Value);
        Assert.Equal(_command.Documento, response.Documento);
        Assert.Equal(_command.Agencia, response.Agencia);
        Assert.Equal(_command.Conta, response.Conta);
        Assert.Equal(_command.Valor, response.Valor);
        _validator.Verify(x => x.ValidateAsync(_command, It.IsAny<CancellationToken>()), Times.Once);
        _handler.Verify(x => x.HandleAsync(_command, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Post_QuandoHandlerFalhar_RetornaBadRequest()
    {
        _validator
            .Setup(v => v.ValidateAsync(_command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _handler
            .Setup(h => h.HandleAsync(_command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<CreateLimiteResponse>().WithError("Erro"));

        var result = await _sut.Post(_command, _validator.Object, _handler.Object, CancellationToken.None);

        StatusCodeResult response = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(400, response.StatusCode);
        _validator.Verify(x => x.ValidateAsync(_command, It.IsAny<CancellationToken>()), Times.Once);
        _handler.Verify(x => x.HandleAsync(_command, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Post_QuandoDadosInvalidos_RetornaPreconditionFailed()
    {
        var command = _command with { Valor = 0.00M };
        _validator
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("any-prop", "any-error-message") }));

        var result = await _sut.Post(command, _validator.Object, _handler.Object, CancellationToken.None);

        ObjectResult response = Assert.IsType<ObjectResult>(result);
        Assert.Equal(412, response.StatusCode);
        _validator.Verify(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
        _handler.Verify(x => x.HandleAsync(command, It.IsAny<CancellationToken>()), Times.Never);
    }
}