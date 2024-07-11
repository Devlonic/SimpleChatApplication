using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.BLL.Models {
    public class PaginatedQueryResult<ResultType> where ResultType : class {
        public int TotalPagesCount { get; set; }
        public int NextPage { get; set; }
        public int PreviousPage { get; set; }
        public IList<ResultType> CurrentPageList { get; set; } = default!;
    }
}
