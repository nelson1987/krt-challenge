using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Domain.Repositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Dependencies
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped<ILimiteRepository, LimiteRepository>();
        return services;
    }

    public static IServiceCollection ConfigureDynamoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var awsConfiguration = configuration.GetSection("AwsConfig");
        var url = awsConfiguration["ServiceURL"];
        var key = awsConfiguration["AccessKey"];
        var secret = awsConfiguration["SecretKey"];

        var amazonDynamoDbConfig = new AmazonDynamoDBConfig();
        var credentials = new BasicAWSCredentials(key, secret);

        var config = new AmazonDynamoDBConfig
        {
            ServiceURL = url
        };
        var dbClient = new AmazonDynamoDBClient(credentials, config);
        services.AddScoped<IAmazonDynamoDB>(_ => dbClient);
        services.AddScoped<IDynamoDBContext>(_ => new DynamoDBContext(new AmazonDynamoDBClient(credentials, config)));

        dbClient.CriarTabelaSeNaoExistir("bounds").GetAwaiter();

        Thread.Sleep(TimeSpan.FromSeconds(40));

        return services;
    }
}

public static class AmazonDynamoDBClientExtensionsV2
{
    public static async Task CriarTabelaSeNaoExistir(this AmazonDynamoDBClient _dynamoDbClient, string nomeTabela)
    {
        Console.WriteLine("CriarTabelaSeNaoExistir");
        var request = new DescribeTableRequest { TableName = nomeTabela };

        try
        {
            await _dynamoDbClient.DescribeTableAsync(request);
            Console.WriteLine($"Tabela {nomeTabela} já existe.");
        }
        catch (ResourceNotFoundException)
        {
            await _dynamoDbClient.CriarTabela(nomeTabela);
            Console.WriteLine($"Tabela {nomeTabela} criada com sucesso.");
        }
        catch (AmazonDynamoDBException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await _dynamoDbClient.CriarTabela(nomeTabela);
                Console.WriteLine($"Tabela {nomeTabela} criada com sucesso.");
            }
            else
            {
                throw;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Tabela {ex} criada com sucesso.");
        }
    }

    private static async Task CriarTabela(this AmazonDynamoDBClient _dynamoDbClient, string nomeTabela)
    {
        Console.WriteLine("CriarTabela");

        var createTableRequest = new CreateTableRequest
        {
            TableName = nomeTabela,
            AttributeDefinitions = new List<AttributeDefinition>()
                              {
                                  new AttributeDefinition
                                  {
                                      AttributeName = "document",
                                      AttributeType = "S"
                                  },
                                  new AttributeDefinition
                                  {
                                      AttributeName = "account",
                                      AttributeType = "S"
                                  }
                              },
            KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement("document", KeyType.HASH),
                    new KeySchemaElement("account", KeyType.RANGE)
                },
            ProvisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 5
            }
        };

        Console.WriteLine("INI|CreateTableAsync");
        await _dynamoDbClient.CreateTableAsync(createTableRequest);

        var tableStatus = "CREATING";
        while (tableStatus == "CREATING")
        {
            await Task.Delay(1000);
            var response = await _dynamoDbClient.DescribeTableAsync(nomeTabela);
            tableStatus = response.Table.TableStatus;
        }
        Console.WriteLine("END|CreateTableAsync");
    }
}