using SimpleChatApplication.DAL.Entities;

namespace SimpleChatApplication.BLL.Models.Lookups {
    public class ChatRoomLookup : BaseLookup<ChatRoomEntity> {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public int? CreatorId { get; set; }
        public UserLookup? Creator { get; set; }
    }
}
