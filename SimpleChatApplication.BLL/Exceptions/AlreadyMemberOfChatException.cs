using SimpleChatApplication.DAL.Entities;

namespace SimpleChatApplication.BLL.Exceptions {
    public class AlreadyMemberOfChatException : ForbiddenAccessException {
        public AlreadyMemberOfChatException(UserEntity user, ChatRoomEntity chatRoom)
        : base($"User {user.UserName} is already member of chat room {chatRoom.Title}") {

        }

    }
}