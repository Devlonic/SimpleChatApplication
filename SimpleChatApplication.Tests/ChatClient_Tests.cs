using MediatR;
using SimpleChatApplication.BLL.CQRS.ChatRooms.Commands;
using SimpleChatApplication.BLL.CQRS.ChatRooms.Notifications;
using SimpleChatApplication.BLL.CQRS.Users.Commands;
using SimpleChatApplication.BLL.Exceptions;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using SimpleChatApplication.BLL;
using SimpleChatApplication.DAL;
using Microsoft.Extensions.Configuration;
using SimpleChatApplication.DAL.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using MediatR;
using SimpleChatApplication.BLL.Services;
using Moq;

namespace SimpleChatApplication.Tests {
    public class ChatClient_Tests {
        [Fact]
        public async Task ShouldThrowsException_WhenUserTriedToJoinChatThatHeIsNotMemberOf() {
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

            // Act

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

            // user1 join to room1
            await Mediator.Publish(new NewUserConnectedToRoomNotification() {
                ChatRoomId = room1,
                UserId = user1Id
            });

            // Assert

            await Assert.ThrowsAsync<NotMemberOfChatException>(async () => {
                await Mediator.Publish(new UserSentMessageToChatRoomNotification() {
                    ChatRoomId = room1,
                    UserId = user2Id,
                    Message = "Haha, i sent to the chat, that i am not a member of xD"
                });
            });
        }

        [Fact]
        public async Task ShouldThrowsException_WhenUserTriedToJoinTwiceToSameChat() {
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

            // Act

            // create 2 users
            var user1Id = await Mediator.Send(new SignInUserCommand() {
                UserName = "UserName_1"
            });

            // create 1 room
            var room1 = await Mediator.Send(new CreateChatRoomUserCommand() {
                CreatorId = user1Id,
                Title = $"ChatRoomTitle_1"
            });

            // user1 join to room1
            await Mediator.Publish(new NewUserConnectedToRoomNotification() {
                ChatRoomId = room1,
                UserId = user1Id
            });

            // Assert

            await Assert.ThrowsAsync<AlreadyMemberOfChatException>(async () => {
                // user1 join to room1 again, when he is already joined
                await Mediator.Publish(new NewUserConnectedToRoomNotification() {
                    ChatRoomId = room1,
                    UserId = user1Id
                });
            });
        }
    }
}