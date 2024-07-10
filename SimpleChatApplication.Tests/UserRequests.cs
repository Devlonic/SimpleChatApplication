using Microsoft.Extensions.DependencyInjection;
using Xunit;
using SimpleChatApplication.BLL;
using SimpleChatApplication.DAL;
using Microsoft.Extensions.Configuration;
using MediatR;
using SimpleChatApplication.BLL.CQRS.Users.Commands;
using SimpleChatApplication.DAL.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
namespace SimpleChatApplication.Tests {
    public class UserRequests {
        [Fact]
        public async Task ShouldReturnTwoSamePrimaryKeys_WhenUsernameIsSame() {
            // Arranging

            // create empty services vault
            IServiceCollection services = new ServiceCollection();

            // setup configuration
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.Tests.json");
            var configuration = configurationBuilder.Build();

            // setup DAL, BLL services
            services.AddBusinessLogicLayerServices(configuration);
            services.AddDataAccessLayerServices(configuration);
            services.AddLogging();
            var serviceProvider = services.BuildServiceProvider();

            // recreate database
            var database = serviceProvider.GetRequiredService<ChatApplicationDbContext>();
            await database.Database.EnsureDeletedAsync();
            await database.Database.MigrateAsync();

            // get mediator instance
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            var command = new SignInUserCommand() {
                UserName = "ShuttleX_BestCompany"
            };

            // Act
            var responce1 = await mediator.Send(command);
            var responce2 = await mediator.Send(command);

            // Assert
            Assert.True(responce1 == responce2, "Primary keys is not same");
        }

        [Fact]
        public async Task ShouldThrowValidationException_WhenUsernameExceedsSizeLimit() {
            // Arranging

            // create empty services vault
            IServiceCollection services = new ServiceCollection();

            // setup configuration
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.Tests.json");
            var configuration = configurationBuilder.Build();

            // setup DAL, BLL services
            services.AddBusinessLogicLayerServices(configuration);
            services.AddDataAccessLayerServices(configuration);
            services.AddLogging();
            var serviceProvider = services.BuildServiceProvider();

            // recreate database
            var database = serviceProvider.GetRequiredService<ChatApplicationDbContext>();
            await database.Database.EnsureDeletedAsync();
            await database.Database.MigrateAsync();

            // get mediator instance
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            var command = new SignInUserCommand() {
                UserName = "ShuttleX_BestCompanyAndIWouldLikeToBeInYourTeam"
            };

            // Act
            // Assert
            await Assert.ThrowsAsync<ValidationException>(async () => {
                await mediator.Send(command);
            });
        }
    }
}
