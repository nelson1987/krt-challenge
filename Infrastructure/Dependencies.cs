using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Domain.Repositories;
using Infrastructure.Extensions;
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
        services.AddScoped<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(credentials, config));
        services.AddScoped<IDynamoDBContext>(_ => new DynamoDBContext(new AmazonDynamoDBClient(credentials, config)));

        dbClient.Migrate()
            .GetAwaiter();

        Thread.Sleep(TimeSpan.FromSeconds(40));

        return services;
    }
}