using Newtonsoft.Json;
using SimpleChatApplication.DAL.Entities;
using System.Text.Json.Serialization;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

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

            [JsonIgnore]
            public UserEntity? User { get; set; }
            [JsonIgnore]
            public ChatRoomEntity? ChatRoom { get; set; }
        }

        public class UserQuitInfo {
            public string UserName { get; set; }
            public int UserId { get; set; }
            public int? ChatRoomId { get; set; }
            public QuitReason UserQuitReason { get; set; }

            [JsonIgnore]
            public UserEntity? User { get; set; }
            [JsonIgnore]
            public ChatRoomEntity? ChatRoom { get; set; }

            public enum QuitReason {
                Unknown,
                Quit,
                ConnectionLost
            }
        }

        public class SendMessageInfo {
            public string UserName { get; set; }
            public int UserId { get; set; }
            public int ChatRoomId { get; set; }
            public string Message { get; set; }

            [JsonIgnore]
            public UserEntity? User { get; set; }
            [JsonIgnore]
            public ChatRoomEntity? ChatRoom { get; set; }
        }

        public class ChatRoomDeletedInfo {
            public int ChatRoomId { get; set; }
            public string? Message { get; set; }

            [JsonIgnore]
            public UserEntity? User { get; set; }
            [JsonIgnore]
            public ChatRoomEntity? ChatRoom { get; set; }
        }
    }
}
