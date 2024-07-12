using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SimpleChatApplication.BLL.CQRS.ChatRooms.Commands;
using SimpleChatApplication.BLL.CQRS.Users.Commands;
using SimpleChatApplication.BLL.Exceptions;
using SimpleChatApplication.BLL.Services;
using SimpleChatApplication.BLL;
using SimpleChatApplication.DAL;
using SimpleChatApplication.DAL.Data.Contexts;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace SimpleChatApplication.Tests {
    public class ChatRequests_Tests {
        [Fact]
        public async Task ShouldThrowsForbidenException_WhenUserTriesToDeleteForeignChat() {
            /// Arrange

            // create empty services vault
            var Services = new ServiceCollection();

            // setup configuration
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.Tests.json");
            var configuration = configurationBuilder.Build();

            // change default connection string name 
            // to prevent parallel multi-test access to same database instance
            configuration["ConnectionStrings:default"] = string.Format(configuration["ConnectionStrings:default"] ?? "", Guid.NewGuid());

            // mock Messenger
            var mock = new Mock<IRealTimeMessenger>();
            mock.Setup(a => a.SendMessageToAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            mock.Setup(a => a.SendMessageToAsync(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // setup DAL, BLL services
            Services.AddBusinessLogicLayerServices(configuration);
            Services.AddDataAccessLayerServices(configuration);
            Services.AddLogging();
            Services.AddSingleton<IRealTimeMessenger>(mock.Object);
            var ServiceProvider = Services.BuildServiceProvider();

            // recreate database
            var database = ServiceProvider.GetRequiredService<ChatApplicationDbContext>();
            await database.Database.EnsureDeletedAsync();
            await database.Database.MigrateAsync();

            // get mediator instance
            var Mediator = ServiceProvider.GetRequiredService<IMediator>();

            /// Act

            // create 2 users
            var user1Id = await Mediator.Send(new SignInUserCommand() {
                UserName = "UserName_1"
            });
            var user2Id = await Mediator.Send(new SignInUserCommand() {
                UserName = "UserName_2"
            });

            // create 1 room
            var room1 = await Mediator.Send(new CreateChatRoomUserCommand() {
                CreatorId = user1Id,
                Title = $"ChatRoomTitle_1"
            });

            /// Assert

            await Assert.ThrowsAsync<ForbiddenAccessException>(async () => {
                await Mediator.Send(new DeleteChatRoomUserCommand() {
                    ChatRoomId = room1,
                    RequesterId = user2Id
                });
            });
        }
    }
}