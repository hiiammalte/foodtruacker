using foodtruacker.API.Extensions;
using foodtruacker.Application.BoundedContexts.UserAccountManagement.Commands;
using foodtruacker.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using foodtruacker.EventSourcingRepository.Repository;
using foodtruacker.EventSourcingRepository.Configuration;
using foodtruacker.QueryRepository.Repository;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using foodtruacker.Authentication.Configuration;
using foodtruacker.Authentication.Repository;
using foodtruacker.QueryRepository.Configuration;
using foodtruacker.Application.Pipelines;
using foodtruacker.Domain.BoundedContexts.TourPricingManagement.Aggregates;
using foodtruacker.API.Middleware;

namespace foodtruacker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddSwaggerDocumentation();

            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.AddJwtBasedAuth(Configuration.GetSection("JwtSettings").Get<JwtSettings>());
            services.AddEventStoreDbService(Configuration.GetSection("EventStoreDb").Get<EventStoreDbSettings>());
            services.AddIdenityServices(Configuration.GetSection("MySqlDb").Get<MySqlSettings>());
            services.AddMongoDbService(Configuration.GetSection("MongoDb").Get<MongoDbSettings>());
            services.AddEmailService();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AdminAccountCreateCommand).Assembly));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MetricsBehaviour<,>));

            services.AddTransient<IEventSourcingRepository<Admin>, EventSourcingRepository<Admin>>();
            services.AddTransient<IProjectionRepository<AdminInfo>, MongoDbRepository<AdminInfo>>();
            services.AddTransient<IQueryRepository<AdminInfo>, MongoDbRepository<AdminInfo>>();

            services.AddTransient<IEventSourcingRepository<Customer>, EventSourcingRepository<Customer>>();
            services.AddTransient<IProjectionRepository<CustomerInfo>, MongoDbRepository<CustomerInfo>>();
            services.AddTransient<IQueryRepository<CustomerInfo>, MongoDbRepository<CustomerInfo>>();

            services.AddTransient<IEventSourcingRepository<TourStopPricingModel>, EventSourcingRepository<TourStopPricingModel>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IdentityContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseGlobalExceptionMiddleware();
            }

            app.UseSwaggerDocumentation();
            app.UseIdentityMySqlMigration(context);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseJwtBasedAuth();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        
    }
}
