using FluentValidation;
using MediatR;
using SimpleChatApplication.BLL.CQRS.ChatRooms.Commands;
using SimpleChatApplication.BLL.Models;
using SimpleChatApplication.BLL.Models.Lookups;
using SimpleChatApplication.DAL.Entities;
using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.BLL.CQRS.ChatRooms.Queries {
    public class GetAllChatRoomsQuery : PaginatedQuery<ChatRoomLookup> {
        public class GetAllChatRoomsQueryHandler : IRequestHandler<GetAllChatRoomsQuery, PaginatedQueryResult<ChatRoomLookup>> {
            private readonly IUnitOfWorkFactory unitOfWorkFactory;

            public GetAllChatRoomsQueryHandler(IUnitOfWorkFactory unitOfWorkFactory) {
                this.unitOfWorkFactory = unitOfWorkFactory;
            }

            public async Task<PaginatedQueryResult<ChatRoomLookup>> Handle(GetAllChatRoomsQuery request, CancellationToken cancellationToken) {
                var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
                var chatRoomRepository = unitOfWork.GetRepository<ChatRoomEntity>();

                var x = await chatRoomRepository.GetAsync<ChatRoomLookup>(
                    filter: (c) => true,
                    selector: (c) => new ChatRoomLookup() {
                        CreatorId = c.CreatorId,
                        Title = c.Title,
                        Id = c.Id,
                        Creator = new UserLookup() {
                            UserName = c.Creator!.UserName
                        }
                    },
                    includeProperties: nameof(ChatRoomEntity.Creator),
                    skip: request.CurrentPage * request.PageSize - 1,
                    take: request.PageSize);

                return new PaginatedQueryResult<ChatRoomLookup>() {
                    CurrentPageList = x.ToList(),
                    NextPage = 0,
                    PreviousPage = 0,
                    TotalPagesCount = 0
                };
            }
        }
    }
}
