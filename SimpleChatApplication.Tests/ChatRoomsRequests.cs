using SimpleChatApplication.BLL.CQRS.ChatRooms.Commands;
using SimpleChatApplication.BLL.CQRS.Users.Commands;
using Xunit;
namespace SimpleChatApplication.Tests {
    public class ChatRoomsRequests : TestBase {
        [Fact]
        public async Task ShouldReturnTwoDifferentKeys_WhenChatTitleIsSame() {
            var userId = await Mediator.Send(new SignInUserCommand() {
                UserName = "Username1"
            });

            var chatId1 = await Mediator.Send(new CreateChatRoomUserCommand() {
                CreatorId = userId,
                Title = "ChatRoom1"
            });


            Assert.True(chatId1 != chatId2, "Chat primary key is same");
        }
    }
}
