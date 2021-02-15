using foodtruacker.Authentication.Configuration;
using foodtruacker.Authentication.Entities;
using foodtruacker.Authentication.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using System;

namespace foodtruacker.API.Extensions
{
    public static class IdentityMySqlServiceExtensions
    {
        public static IServiceCollection AddIdenityServices(this IServiceCollection services, MySqlSettings settings)
        {
            var connectionStringVuilder = new MySqlConnectionStringBuilder()
            {
                Server = settings.Url,
                Database = settings.Database,
                Port = (uint)settings.Port,
                Password = settings.Password,
                UserID = settings.Username,
                Pooling = false,
                SslMode = MySqlSslMode.Required
            };

            services.AddDbContext<IdentityContext>(o =>
                {
                    o.UseMySql
                    (
                        connectionStringVuilder.ConnectionString,
                        new MySqlServerVersion(new Version(8, 0, 21)),
                        b => b.MigrationsAssembly("foodtruacker.Authentication")
                    );
                    o.EnableDetailedErrors();
                },
                ServiceLifetime.Scoped);

            services.AddIdentity<User, Role>(o =>
                {
                    // Password settings.
                    o.Password.RequiredLength = 6;
                    o.Password.RequireDigit = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequireUppercase = false;

                    // User settings.
                    o.User.RequireUniqueEmail = true;

                    // SignIn settings.
                    o.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

            return services;
        }

        public static IApplicationBuilder UseIdentityMySqlMigration(this IApplicationBuilder app, IdentityContext context)
        {
            context.Database.Migrate();

            return app;
        }
    }
}
