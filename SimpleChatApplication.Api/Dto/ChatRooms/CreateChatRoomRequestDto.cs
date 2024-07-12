using Microsoft.AspNetCore.Mvc;

namespace SimpleChatApplication.Api.Dto.ChatRooms {
    public class CreateChatRoomRequestDto {
        public string? Title { get; set; } = null!;
    }
    public class CreateChatRoomResponceDto {
        public int CreatedChatId { get; set; }
    }
}
