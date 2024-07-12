using Microsoft.Extensions.DependencyInjection;
using SimpleChatApplication.BLL;
using SimpleChatApplication.DAL;
using Microsoft.Extensions.Configuration;
using SimpleChatApplication.DAL.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using MediatR;
namespace SimpleChatApplication.Tests {
    public class TestBase {
        public TestBase() {
            // prepare all required enviroment for passing tests
            Task.Run(PrepareEnviroment).Wait();
        }
        ~TestBase() {
            Task.Run(CleanUpEnviroment).Wait();
        }
        protected IServiceProvider ServiceProvider { get; private set; }
        protected IServiceCollection Services { get; private set; }
        protected IMediator Mediator { get; private set; }

        public async Task PrepareEnviroment() {
            // create empty services vault
            Services = new ServiceCollection();

            // setup configuration
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.Tests.json");
            var configuration = configurationBuilder.Build();

            // change default connection string name 
            // to prevent parallel multi-test access to same database instance
            configuration["ConnectionStrings:default"] = string.Format(configuration["ConnectionStrings:default"] ?? "", Guid.NewGuid());

            // setup DAL, BLL services
            Services.AddBusinessLogicLayerServices(configuration);
            Services.AddDataAccessLayerServices(configuration);
            Services.AddLogging();
            ServiceProvider = Services.BuildServiceProvider();

            // recreate database
            var database = ServiceProvider.GetRequiredService<ChatApplicationDbContext>();
            await database.Database.EnsureDeletedAsync();
            await database.Database.MigrateAsync();

            // get mediator instance
            Mediator = ServiceProvider.GetRequiredService<IMediator>();
        }
        protected void RebuildServiceProvider() {
            ServiceProvider = Services.BuildServiceProvider();
        }
        public async Task CleanUpEnviroment() {
            var database = ServiceProvider.GetRequiredService<ChatApplicationDbContext>();
            await database.Database.EnsureDeletedAsync();
        }
    }
}
