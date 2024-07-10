using FluentValidation;
using MediatR;
using SimpleChatApplication.DAL.Entities;
using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.BLL.CQRS.Users.Commands {
    public class SignInUserCommand : IRequest<int> {
        public string UserName { get; set; } = null!;

        public class SignInUserCommandHandler : IRequestHandler<SignInUserCommand, int> {
            private readonly IUnitOfWorkFactory unitOfWorkFactory;

            public SignInUserCommandHandler(IUnitOfWorkFactory unitOfWorkFactory) {
                this.unitOfWorkFactory = unitOfWorkFactory;
            }

            public async Task<int> Handle(SignInUserCommand request, CancellationToken cancellationToken) {
                using var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
                var repository = unitOfWork.GetRepository<UserEntity>();

                var existingUser = await repository
                    .GetFirstByFilter(u => u.UserName == request.UserName);

                if ( existingUser is not null )
                    return existingUser.Id;

                var user = new UserEntity() {
                    UserName = request.UserName,
                };
                await repository.InsertAsync(user);

                await unitOfWork.CommitAsync();
                return user.Id;
            }
        }
        public class SignInUserCommandValidator : AbstractValidator<SignInUserCommand> {
            public SignInUserCommandValidator() {
                RuleFor(c => c.UserName)
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(24)
                    .Matches(@"^[\d\w\W_\s]{0,}$");
            }
        }
    }
}
