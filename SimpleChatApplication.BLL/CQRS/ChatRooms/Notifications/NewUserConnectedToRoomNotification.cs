﻿using MediatR;
using SimpleChatApplication.BLL.CQRS.Events;
using SimpleChatApplication.BLL.Models.EventTypes;
using SimpleChatApplication.BLL.Services;
using SimpleChatApplication.DAL.Entities;
using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.BLL.CQRS.ChatRooms.Notifications {
    public class NewUserConnectedToRoomNotification : INotification {
        public int ChatRoomId { get; set; }
        public int UserId { get; set; }

        public class NewUserConnectedToRoomNotificationRealTimeHandler : INotificationHandler<NewUserConnectedToRoomNotification> {
            private readonly IChatService chatService;

            //private readonly IEventPublisher<ChatMessageEvent> _eventPublisher;
            private readonly IUnitOfWorkFactory unitOfWorkFactory;

            public NewUserConnectedToRoomNotificationRealTimeHandler(IChatService chatService, IUnitOfWorkFactory unitOfWorkFactory) {
                this.chatService = chatService;
                //_eventPublisher = eventPublisher;
                this.unitOfWorkFactory = unitOfWorkFactory;
            }

            public async Task Handle(NewUserConnectedToRoomNotification notification, CancellationToken cancellationToken) {
                var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
                var user = await unitOfWork.GetRepository<UserEntity>().GetByIdAsync(notification.UserId);
                var chatRoom = await unitOfWork.GetRepository<ChatRoomEntity>().GetByIdAsync(notification.ChatRoomId);

                var chatMessageEvent = new ChatMessageEvent() {
                    MessageType = ChatMessageEvent.ChatMessageType.UserJoin,
                    MessageBody = new ChatMessageEvent.UserJoinInfo {
                        UserName = user.UserName,
                        UserId = user.Id,
                        ChatRoomId = chatRoom.Id,
                        ChatRoom = chatRoom,
                        User = user
                    }
                };
                await chatService.JoinToRoomAsync(chatMessageEvent);
                //await _eventPublisher.SendEventAsync(chatMessageEvent);
            }
        }
    }
}
