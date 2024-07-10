using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleChatApplication.DAL.Data.Contexts;
using SimpleChatApplication.DAL.Data.UnitOfWorks;
using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.DAL {
    public static class ConfigureServices {
        public static IServiceCollection AddDataAccessLayerServices(this IServiceCollection services, IConfiguration configuration) {
            var connectionString = configuration.GetConnectionString("default");

            // ensure that connection string is available in configuration,
            // otherwise throw exception
            Guard.Against.Null(connectionString, message: $"{nameof(connectionString)} is missing in current configuration");

            services.AddDbContext<ChatApplicationDbContext>((sp, options) => {
                options.UseSqlite(connectionString);
            });

            // Add Data Access Tier services
            services.AddScoped<IUnitOfWorkFactory>(sp =>
                new UnitOfWorkFactory(sp.GetRequiredService<ChatApplicationDbContext>()));

            return services;
        }
    }
}
