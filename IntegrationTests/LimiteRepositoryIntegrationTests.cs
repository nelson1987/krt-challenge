using Amazon.DynamoDBv2;
using Amazon.Runtime;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.DTOs;
using Infrastructure.Repositories;

namespace IntegrationTests;

public class LimiteRepositoryIntegrationTests
{
    private readonly LimiteRepository _repository;

    private IFixture _fixture = new Fixture()
        .Customize(new AutoMoqCustomization
        {
            ConfigureMembers = true
        });

    public LimiteRepositoryIntegrationTests()
    {
        //var awsConfiguration = configuration.GetSection("AwsConfig");
        //var url = awsConfiguration["ServiceURL"];
        //var key = awsConfiguration["AccessKey"];
        //var secret = awsConfiguration["SecretKey"];
        var url = "http://localhost:8000";
        var key = "myAcessKeyId";
        var secret = "secretAcessKey";

        var amazonDynamoDbConfig = new AmazonDynamoDBConfig();
        var credentials = new BasicAWSCredentials(key, secret);

        var config = new AmazonDynamoDBConfig
        {
            ServiceURL = url
        };
        IAmazonDynamoDB dynamoDb = new AmazonDynamoDBClient(credentials, config);
        _repository = new LimiteRepository(dynamoDb);
    }

    [Fact]
    public async Task FluxoCompleto()
    {
        LimiteDto limiteDto = _fixture
            .Build<LimiteDto>()
            .Create();
        var limiteBuscado = await _repository.GetAsync(limiteDto.Document, limiteDto.Branch, limiteDto.Account, CancellationToken.None);
        Assert.Null(limiteBuscado);

        var limiteCriado = await _repository.InsertAsync(limiteDto, CancellationToken.None);
        Assert.Equal(limiteDto, limiteCriado);

        limiteBuscado = await _repository.GetAsync(limiteDto.Document, limiteDto.Branch, limiteDto.Account, CancellationToken.None);
        Assert.NotNull(limiteBuscado);

        limiteDto = limiteDto with { Value = 0.02M };
        var limiteAlterado = await _repository.UpdateAsync(limiteDto, CancellationToken.None);
        Assert.Equal(limiteDto, limiteAlterado);

        limiteBuscado = await _repository.GetAsync(limiteDto.Document, limiteDto.Branch, limiteDto.Account, CancellationToken.None);
        Assert.NotNull(limiteBuscado);

        await _repository.DeleteAsync(limiteDto.Document, limiteDto.Account, CancellationToken.None);
        limiteBuscado = await _repository.GetAsync(limiteDto.Document, limiteDto.Branch, limiteDto.Account, CancellationToken.None);
        Assert.Null(limiteBuscado);
    }
}