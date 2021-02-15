using EventStore.Client;
using foodtruacker.EventSourcingRepository.Client;
using foodtruacker.EventSourcingRepository.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Security;

namespace foodtruacker.API.Extensions
{
    public static class EventStoreDbServiceExtensions
    {
        public static IServiceCollection AddEventStoreDbService(this IServiceCollection services, EventStoreDbSettings settings)
        {

            var eventStoreClientSettings = new EventStoreClientSettings
            {
                ConnectivitySettings =
                {
                    Address = new Uri(settings.Uri)
                },
                DefaultCredentials = new UserCredentials(settings.Username, settings.Password),
                CreateHttpMessageHandler = () =>
                new SocketsHttpHandler
                {
                    SslOptions = new SslClientAuthenticationOptions
                    {
                        RemoteCertificateValidationCallback = delegate { return true; }
                    }
                }
            };

            services.AddSingleton(_ => new EventStoreClient(eventStoreClientSettings)).BuildServiceProvider();
            services.AddScoped<IEventSourcingClient, EventStoreDbClient>();

            return services;
        }
    }
}
