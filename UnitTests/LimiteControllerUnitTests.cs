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
        _sut = _fixture.Build<LimiteController>().OmitAutoProperties().Create();
    }

    [Fact]
    public async Task Post_QuandoDadosValidos_RetornaOk()
    {
        _validator
                .Setup(v => v.ValidateAsync(_command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

        var handlerResult = new Result<CreateLimiteResponse>();
        handlerResult.WithValue(new CreateLimiteResponse(Guid.NewGuid(), _command.Documento, _command.Agencia, _command.Conta, _command.Valor));
        _handler.Setup(h => h.Handle(_command, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(handlerResult);

        var postResult = await _sut.Post(_command, _validator.Object, _handler.Object, CancellationToken.None);
        ObjectResult result = Assert.IsType<ObjectResult>(postResult);
        Assert.Equal(200, result.StatusCode);
        CreateLimiteResponse response = Assert.IsType<CreateLimiteResponse>(result.Value);
        Assert.NotEqual(Guid.NewGuid(), response.Id);
        Assert.Equal(_command.Documento, response.Documento);
        Assert.Equal(_command.Agencia, response.Agencia);
        Assert.Equal(_command.Conta, response.Conta);
        Assert.Equal(_command.Valor, response.Valor);
    }

    [Fact]
    public async Task Post_QuandoHandlerFalhar_RetornaBadRequest()
    {
        _validator
                .Setup(v => v.ValidateAsync(_command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

        var handlerResult = new Result<CreateLimiteResponse>();
        handlerResult.WithError("Erro");
        _handler.Setup(h => h.Handle(_command, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(handlerResult);

        var result = await _sut.Post(_command, _validator.Object, _handler.Object, CancellationToken.None);
        StatusCodeResult response = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(404, response.StatusCode);
    }

    [Fact]
    public async Task Post_QuandoDadosInvalidos_RetornaPreconditionFailed()
    {
        var command = _command with { Valor = 0.00M };
        _validator
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult() { Errors = new List<ValidationFailure>() });

        var handlerResult = new Result<CreateLimiteResponse>();
        handlerResult.WithSuccess("Sucesso");
        _handler.Setup(h => h.Handle(command, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(handlerResult);

        var result = await _sut.Post(command, _validator.Object, _handler.Object, CancellationToken.None);
        StatusCodeResult response = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(412, response.StatusCode);
    }
}