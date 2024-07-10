using FluentValidation;
using MediatR;
using SimpleChatApplication.DAL.Entities;
using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.BLL.CQRS.ChatRooms.Commands {
    public class CreateChatRoomUserCommand : IRequest<int> {
        public string? Title { get; set; } = null!;
        public int? CreatorId { get; set; } = null!;

        public class CreateChatRoomUserHandler : IRequestHandler<CreateChatRoomUserCommand, int> {
            private readonly IUnitOfWorkFactory unitOfWorkFactory;

            public CreateChatRoomUserHandler(IUnitOfWorkFactory unitOfWorkFactory) {
                this.unitOfWorkFactory = unitOfWorkFactory;
            }

            public async Task<int> Handle(CreateChatRoomUserCommand request, CancellationToken cancellationToken) {
                var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
                var userRepository = unitOfWork.GetRepository<UserEntity>();
                var chatRoomRepository = unitOfWork.GetRepository<ChatRoomEntity>();

                var user = await userRepository
                    .GetByIdAsync(request.CreatorId ?? -1);

                var room = new ChatRoomEntity() {
                    Creator = user,
                    CreatorId = request.CreatorId,
                    Title = request.Title,
                };

                await chatRoomRepository.InsertAsync(room);

                await unitOfWork.CommitAsync();
                return room.Id;
            }
        }
        public class CreateChatRoomUserCommandValidator : AbstractValidator<CreateChatRoomUserCommand> {
            public CreateChatRoomUserCommandValidator() {
                RuleFor(c => c.Title)
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(64)
                    .Matches(@"^[\d\w\W_\s]{0,}$");
            }
        }
    }
}
