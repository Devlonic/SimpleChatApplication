namespace SimpleChatApplication.Api.Dto.ChatRooms {
    public class CreateChatRoomRequestDto {
        public int UserId { get; set; }

        public int Id { get; set; }
        public string? Title { get; set; } = null!;
    }
    public class CreateChatRoomResponceDto {
        public int CreatedChatId { get; set; }
    }
}
