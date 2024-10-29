using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Domain.Entities;
using Domain.Helpers;
using Infrastructure.Repositories;

namespace IntegrationTests;

public class LimiteRepositoryIntegrationTests
{
    private readonly LimiteRepository _repository;

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
        /*
         * Busar um dado
         * Criar um dado
         * Buscar o dado
         * Alterar o dado
         * Buscar o dado
         * Deletar o dado
         * Buscar o dado
         */
        var limiteEntity = new Limite("Documento", "Agencia", "Conta", 0.01M);
        var limite = limiteEntity.ToDto();
        var limiteBuscado = await _repository.Buscar(limiteEntity.Documento, limiteEntity.Agencia, limiteEntity.Conta, CancellationToken.None);
        Assert.Null(limiteBuscado);

        var limiteCriado = await _repository.Incluir(limite, CancellationToken.None);
        Assert.Equal(limite, limiteCriado);

        limiteBuscado = await _repository.Buscar(limiteEntity.Documento, limiteEntity.Agencia, limiteEntity.Conta, CancellationToken.None);
        Assert.NotNull(limiteBuscado);

        //limiteBuscado.AlterarLimite(0.01M);
    }
}