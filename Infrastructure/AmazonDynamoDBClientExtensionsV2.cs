using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Infrastructure;

public static class AmazonDynamoDBClientExtensionsV2
{
    public static async Task Migrate(this AmazonDynamoDBClient _dynamoDbClient)
    {
        await _dynamoDbClient.CriarTabelaSeNaoExistir("bounds");
    }

    private static async Task CriarTabelaSeNaoExistir(this AmazonDynamoDBClient _dynamoDbClient, string nomeTabela)
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