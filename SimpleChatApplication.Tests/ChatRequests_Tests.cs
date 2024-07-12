using SimpleChatApplication.BLL.CQRS.ChatRooms.Commands;
using SimpleChatApplication.BLL.CQRS.Users.Commands;
using SimpleChatApplication.BLL.Exceptions;
using Xunit;

namespace SimpleChatApplication.Tests {
    public class ChatRequests_Tests : TestBase {
        [Fact]
        public async Task ShouldThrowsForbidenException_WhenUserTriesToDeleteForeignChat() {
            /// Arrange

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