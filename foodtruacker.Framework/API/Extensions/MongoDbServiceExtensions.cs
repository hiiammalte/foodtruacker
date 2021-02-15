using foodtruacker.QueryRepository.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace foodtruacker.API.Extensions
{
    public static class MongoDbServiceExtensions
    {
        public static IServiceCollection AddMongoDbService(this IServiceCollection services, MongoDbSettings settings)
        {
            services.AddSingleton(x => new MongoClient($"mongodb://{settings.Username}:{settings.Password}@{settings.Url}:{settings.Port}/"));
            services.AddSingleton(x => x.GetService<MongoClient>().GetDatabase(settings.Database));

            return services;
        }
    }
}
