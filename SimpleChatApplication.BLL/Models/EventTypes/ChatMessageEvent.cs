using System.Text.Json.Serialization;

namespace SimpleChatApplication.BLL.Models.EventTypes {
    public class ChatMessageEvent {
        public enum ChatMessageType {
            UserJoin,
            UserExit,
            NewMessageReceived,
            ChatDeleted
        }

        public ChatMessageType MessageType { get; set; }
        public object? MessageBody { get; set; }

        public class UserJoinInfo {
            public string UserName { get; set; }
            public int UserId { get; set; }
            public int ChatRoomId { get; set; }
        }
    }
}
