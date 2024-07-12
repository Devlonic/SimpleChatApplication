using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SimpleChatApplication.Api.Dto.ChatRooms;
using SimpleChatApplication.BLL.CQRS.ChatRooms.Notifications;
namespace SimpleChatApplication.Api.Hubs {
    /// <summary>
    /// To access this hub you should provide in query params
    /// user id such as:
    /// /chat?userId=123
    /// </summary>
    public class ChatHub : Hub {
        private readonly IMediator mediator;

        public ChatHub(IMediator mediator) {
            this.mediator = mediator;
        }

        public async Task SendMessage(SendMessageToChatRoomRequestDto dto) {
            var userId = int.Parse(Context.UserIdentifier ?? "");

            await mediator.Publish(new UserSentMessageToChatRoomNotification() {
                ChatRoomId = dto.ChatRoomId,
                Message = dto.MessageText,
                UserId = userId
            });
        }
        public async Task JoinToChatRoom(JoinToChatRoomRequestDto dto) {
            var userId = int.Parse(Context.UserIdentifier ?? "");

            await mediator.Publish(new NewUserConnectedToRoomNotification() {
                ChatRoomId = dto.ChatRoomId,
                UserId = userId
            });
        }
        public async Task QuitChatRoom(JoinToChatRoomRequestDto dto) {
            var userId = int.Parse(Context.UserIdentifier ?? "");

            await mediator.Publish(new UserQuitChatRoomNotification() {
                ChatRoomId = dto.ChatRoomId,
                UserId = userId,
                QuitReason = BLL.Models.EventTypes.ChatMessageEvent.UserQuitInfo.QuitReason.Quit
            });
        }

        public override async Task OnDisconnectedAsync(Exception? exception) {
            var userId = int.Parse(Context.UserIdentifier ?? "-1");
            if ( userId == -1 )
                return;

            await mediator.Publish(new UserQuitChatRoomNotification() {
                ChatRoomId = null, // we dont know, does user is member of any chat rooms
                UserId = userId,
                QuitReason = BLL.Models.EventTypes.ChatMessageEvent.UserQuitInfo.QuitReason.ConnectionLost
            });
        }
    }
}
