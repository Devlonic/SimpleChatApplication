using SimpleChatApplication.DAL.Entities;

namespace SimpleChatApplication.BLL.Exceptions {
    public class NotMemberOfChatException : ForbiddenAccessException {
        public NotMemberOfChatException(UserEntity user, ChatRoomEntity chatRoom)
        : base($"User {user.UserName} is not member of chat room {chatRoom.Title}") {

        }

    }
}