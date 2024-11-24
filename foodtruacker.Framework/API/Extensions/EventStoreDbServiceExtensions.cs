using EventStore.Client;
using foodtruacker.EventSourcingRepository.Client;
using foodtruacker.EventSourcingRepository.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace foodtruacker.API.Extensions
{
    public static class EventStoreDbServiceExtensions
    {
        public static IServiceCollection AddEventStoreDbService(this IServiceCollection services, EventStoreDbSettings settings)
        {
            string connectionString = $"{settings.Schema}{settings.Username}:{settings.Password}@{settings.Url}:{settings.Port}?tls={settings.Tls}&tlsVerifyCert={settings.TlsVerifyCert}";
            var eventStoreClientSettings = EventStoreClientSettings.Create(connectionString);

            services.AddSingleton(_ => new EventStoreClient(eventStoreClientSettings)).BuildServiceProvider();
            services.AddScoped<IEventSourcingClient, EventStoreDbClient>();

            return services;
        }
    }
}
