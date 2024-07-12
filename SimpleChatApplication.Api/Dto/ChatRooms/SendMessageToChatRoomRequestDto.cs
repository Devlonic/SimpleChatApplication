namespace SimpleChatApplication.Api.Dto.ChatRooms {
    public class SendMessageToChatRoomRequestDto {
        public int ChatRoomId { get; set; }
        public string MessageText { get; set; }
    }
}
