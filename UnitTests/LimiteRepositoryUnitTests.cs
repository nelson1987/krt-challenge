using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Repositories;
using Moq;
using System.Net;

namespace UnitTests;

public class LimiteRepositoryUnitTests : UnitTestsBase
{
    private readonly IFixture _fixture = new Fixture()
        .Customize(new AutoMoqCustomization
        {
            ConfigureMembers = true
        });

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
        var limite = new Limite("Documento", "Agencia", "Conta", 0.00M);

        limite = await _sut.Incluir(limite);

        Assert.NotNull(limite);
        Assert.NotEqual(Guid.Empty, limite.Id);
        Assert.Equal("Documento", limite.Documento);
        Assert.Equal("Agencia", limite.Agencia);
        Assert.Equal("Conta", limite.Conta);
        Assert.Equal(0.00M, limite.Valor);

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
        var limite = new Limite("Documento", "Agencia", "Conta", 0.00M);

        await Assert.ThrowsAsync<ContextoException>(() => _sut.Incluir(limite));
        _fixture.Freeze<Mock<IAmazonDynamoDB>>()
                .Verify(x =>
                    x.PutItemAsync(
                        It.IsAny<PutItemRequest>(),
                        It.IsAny<CancellationToken>())
                    , Times.Once);
    }
}