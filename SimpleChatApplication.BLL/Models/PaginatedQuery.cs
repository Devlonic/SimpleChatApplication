using MediatR;
using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.BLL.Models {
    public abstract class PaginatedQuery<ReturnType> : IRequest<PaginatedQueryResult<ReturnType>> where ReturnType : class {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
