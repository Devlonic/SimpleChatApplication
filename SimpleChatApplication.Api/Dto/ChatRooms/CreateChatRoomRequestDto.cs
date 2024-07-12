using Microsoft.AspNetCore.Mvc;

namespace SimpleChatApplication.Api.Dto.ChatRooms {
    public class CreateChatRoomRequestDto {
        public int UserId { get; set; }
        public string? Title { get; set; } = null!;
    }
    public class CreateChatRoomResponceDto {
        public int CreatedChatId { get; set; }
    }

    public class JoinToChatRoomRequestDto {
        public int ChatRoomId { get; set; }
    }
    public class SendMessageToChatRoomRequestDto {
        public int ChatRoomId { get; set; }
        public string MessageText { get; set; }
    }
}
