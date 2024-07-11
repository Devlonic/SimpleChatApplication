using SimpleChatApplication.DAL.Entities;

namespace SimpleChatApplication.BLL.Models.Lookups {
    public class UserLookup : BaseLookup<UserEntity> {
        public int? Id { get; set; }
        public string? UserName { get; set; }
    }
}
