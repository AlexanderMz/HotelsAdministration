using HotelsAdministration.Application.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HotelsAdministration.WebApi.Extension;

public static class PersistenceExtension
{
    public static void Persistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            var client = new MongoClient(settings.ConnectionString);
            return client.GetDatabase(settings.DatabaseName);
        });
    }
}
