using MediatR;
using SimpleChatApplication.BLL.Models.EventTypes;
using SimpleChatApplication.BLL.Services;
using SimpleChatApplication.DAL.Entities;
using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.BLL.CQRS.ChatRooms.Notifications {
    public class UserQuitChatRoomNotification : INotification {
        public int? ChatRoomId { get; set; }
        public int UserId { get; set; }
        public ChatMessageEvent.UserQuitInfo.QuitReason QuitReason { get; set; }

        public class UserQuitChatRoomNotificationRealTimeHandler : INotificationHandler<UserQuitChatRoomNotification> {
            private readonly IChatService chatService;
            private readonly IUnitOfWorkFactory unitOfWorkFactory;

            public UserQuitChatRoomNotificationRealTimeHandler(IChatService chatService, IUnitOfWorkFactory unitOfWorkFactory) {
                this.chatService = chatService;
                this.unitOfWorkFactory = unitOfWorkFactory;
            }

            public async Task Handle(UserQuitChatRoomNotification notification, CancellationToken cancellationToken) {
                var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
                var user = await unitOfWork.GetRepository<UserEntity>().GetByIdAsync(notification.UserId);

                ChatRoomEntity? chatRoom = null;
                if ( notification.ChatRoomId is not null )
                    chatRoom = await unitOfWork.GetRepository<ChatRoomEntity>().GetByIdAsync(notification.ChatRoomId.Value);

                var chatMessageEvent = new ChatMessageEvent() {
                    MessageType = ChatMessageEvent.ChatMessageType.UserExit,
                    MessageBody = new ChatMessageEvent.UserQuitInfo {
                        UserName = user.UserName ?? "",
                        UserId = user.Id,
                        ChatRoomId = notification.ChatRoomId ?? null,
                        ChatRoom = chatRoom,
                        User = user,
                        UserQuitReason = notification.QuitReason
                    }
                };
                await chatService.QuitFromRoomAsync(chatMessageEvent);
            }
        }
    }
}
