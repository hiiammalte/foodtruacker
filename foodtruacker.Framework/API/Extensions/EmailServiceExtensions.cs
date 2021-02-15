using foodtruacker.EmailService.Services;
using Microsoft.Extensions.DependencyInjection;

namespace foodtruacker.API.Extensions
{
    public static class EmailServiceExtensions
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, DemoEmailService>();

            return services;
        }
    }
}
