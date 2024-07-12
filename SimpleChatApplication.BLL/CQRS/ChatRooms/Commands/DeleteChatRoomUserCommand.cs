using FluentValidation;
using MediatR;
using SimpleChatApplication.BLL.Exceptions;
using SimpleChatApplication.BLL.Models.EventTypes;
using SimpleChatApplication.BLL.Services;
using SimpleChatApplication.DAL.Entities;
using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.BLL.CQRS.ChatRooms.Commands {
    public class DeleteChatRoomUserCommand : IRequest {
        public int RequesterId { get; set; }
        public int ChatRoomId { get; set; }

        public class DeleteChatRoomUserCommandHandler : IRequestHandler<DeleteChatRoomUserCommand> {
            private readonly IUnitOfWorkFactory unitOfWorkFactory;
            private readonly IChatService chatService;

            public DeleteChatRoomUserCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IChatService chatService) {
                this.unitOfWorkFactory = unitOfWorkFactory;
                this.chatService = chatService;
            }

            public async Task Handle(DeleteChatRoomUserCommand request, CancellationToken cancellationToken) {
                var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
                var chatRoomRepository = unitOfWork.GetRepository<ChatRoomEntity>();

                var chatRoom = await chatRoomRepository.GetByIdAsync(request.ChatRoomId);

                // room can be deleted only by creator
                if ( chatRoom.CreatorId != request.RequesterId )
                    throw new ForbiddenAccessException(
                        requesterId: request.RequesterId,
                        requestedKey: request.ChatRoomId,
                        entityName: nameof(ChatRoomEntity));

                await chatService.DeleteChatRoomAsync(new Models.EventTypes.ChatMessageEvent() {
                    MessageType = Models.EventTypes.ChatMessageEvent.ChatMessageType.ChatDeleted,
                    MessageBody = new ChatMessageEvent.ChatRoomDeletedInfo() {
                        ChatRoomId = chatRoom.Id,
                        Message = "Chat room was deleted"
                    }
                });

                await chatRoomRepository.DeleteAsync(chatRoom);

                await unitOfWork.CommitAsync();
                return;
            }
        }
    }
}
