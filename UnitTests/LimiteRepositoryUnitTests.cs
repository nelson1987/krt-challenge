using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AutoFixture;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Helpers;
using Infrastructure.Repositories;
using Moq;
using System.Net;

namespace UnitTests;

public class LimiteRepositoryUnitTests : UnitTestsBase
{
    private readonly LimiteRepository _sut;

    public LimiteRepositoryUnitTests()
    {
        _fixture.Freeze<Mock<IAmazonDynamoDB>>()
            .Setup(client => client.PutItemAsync(
               It.IsAny<PutItemRequest>(),
               It.IsAny<CancellationToken>()))
               .ReturnsAsync((PutItemRequest r, CancellationToken token) =>
               {
                   return new PutItemResponse { HttpStatusCode = HttpStatusCode.OK };
               });

        _sut = _fixture.Create<LimiteRepository>();
    }

    [Fact]
    public async Task IncluirLimite_DadosValidos_RetornaDadosInseridos()
    {
        var limite = new Limite("Documento", "Agencia", "Conta", 0.01M);

        var result = await _sut.Incluir(limite.ToDto(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(limite.Documento, result.Document);
        Assert.Equal(limite.Agencia, result.Branch);
        Assert.Equal(limite.Conta, result.Account);
        Assert.Equal(limite.Valor, result.Value);

        _fixture.Freeze<Mock<IAmazonDynamoDB>>()
                .Verify(x =>
                    x.PutItemAsync(
                        It.IsAny<PutItemRequest>(),
                        It.IsAny<CancellationToken>())
                    , Times.Once);
    }

    [Fact]
    public async Task IncluirLimite_ProblemaContexto_DisparaExcecaoContexto()
    {
        _fixture.Freeze<Mock<IAmazonDynamoDB>>()
            .Setup(client => client.PutItemAsync(
               It.IsAny<PutItemRequest>(),
               It.IsAny<CancellationToken>()))
               .ReturnsAsync((PutItemRequest r, CancellationToken token) =>
               {
                   return new PutItemResponse { HttpStatusCode = HttpStatusCode.BadRequest };
               });
        var limite = new Limite("Documento", "Agencia", "Conta", 0.01M);

        await Assert.ThrowsAsync<ContextoException>(() => _sut.Incluir(limite.ToDto()));
        _fixture.Freeze<Mock<IAmazonDynamoDB>>()
                .Verify(x =>
                    x.PutItemAsync(
                        It.IsAny<PutItemRequest>(),
                        It.IsAny<CancellationToken>())
                    , Times.Once);
    }
}