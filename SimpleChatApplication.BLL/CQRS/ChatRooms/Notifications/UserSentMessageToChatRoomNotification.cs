using MediatR;
using SimpleChatApplication.BLL.Models.EventTypes;
using SimpleChatApplication.BLL.Services;
using SimpleChatApplication.DAL.Entities;
using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.BLL.CQRS.ChatRooms.Notifications {
    public class UserSentMessageToChatRoomNotification : INotification {
        public int ChatRoomId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; } = null!;

        public class UserSentMessageToChatRoomNotificationHandler : INotificationHandler<UserSentMessageToChatRoomNotification> {
            private readonly IChatService chatService;
            private readonly IUnitOfWorkFactory unitOfWorkFactory;

            public UserSentMessageToChatRoomNotificationHandler(IChatService chatService, IUnitOfWorkFactory unitOfWorkFactory) {
                this.chatService = chatService;
                this.unitOfWorkFactory = unitOfWorkFactory;
            }

            public async Task Handle(UserSentMessageToChatRoomNotification notification, CancellationToken cancellationToken) {
                var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
                var user = await unitOfWork.GetRepository<UserEntity>().GetByIdAsync(notification.UserId);
                var chatRoom = await unitOfWork.GetRepository<ChatRoomEntity>().GetByIdAsync(notification.ChatRoomId);

                var chatMessageEvent = new ChatMessageEvent() {
                    MessageType = ChatMessageEvent.ChatMessageType.NewMessageReceived,
                    MessageBody = new ChatMessageEvent.SendMessageInfo {
                        UserName = user.UserName,
                        UserId = user.Id,
                        ChatRoomId = chatRoom.Id,
                        ChatRoom = chatRoom,
                        User = user,
                        Message = notification.Message,
                    }
                };
                await chatService.SendMessageAsync(chatMessageEvent);
            }
        }
    }
}
