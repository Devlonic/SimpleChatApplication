using SimpleChatApplication.BLL.CQRS.ChatRooms.Commands;
using SimpleChatApplication.BLL.CQRS.ChatRooms.Queries;
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

            var chatId2 = await Mediator.Send(new CreateChatRoomUserCommand() {
                CreatorId = userId,
                Title = "ChatRoom1"
            });

            Assert.True(chatId1 != chatId2, "Chat primary key is same");
        }

        [Fact]
        public async Task ShouldReturnPaginatedList() {
            // create user
            var userId = await Mediator.Send(new SignInUserCommand() {
                UserName = "Username1"
            });

            // create 10 chat rooms
            for ( int i = 0; i < 10; i++ ) {
                await Mediator.Send(new CreateChatRoomUserCommand() {
                    CreatorId = userId,
                    Title = $"ChatRoom{i}"
                });
            }

            var chatList = await Mediator.Send(new GetAllChatRoomsQuery() {
                CurrentPage = 1,
                PageSize = 2,
            });

            Assert.True(chatList.CurrentPageList.Count == 2, "Pagination not works");
        }
    }
}
