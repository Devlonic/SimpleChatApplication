using Ardalis.GuardClauses;
using SimpleChatApplication.BLL.Exceptions;
using SimpleChatApplication.BLL.Models.EventTypes;
using SimpleChatApplication.BLL.Services;
using SimpleChatApplication.DAL.Entities;
using static SimpleChatApplication.BLL.Models.EventTypes.ChatMessageEvent;

namespace SimpleChatApplication.BLL.Services {
    public class RealTimeChatService : IChatService {
        private readonly Dictionary<int, (ChatRoomEntity ChatRoom, IList<UserEntity> ActiveUserLists)> activeRooms = new();
        private readonly IRealTimeMessenger messenger;

        public RealTimeChatService(IRealTimeMessenger messenger) {
            this.messenger = messenger;
        }

        public async Task JoinToRoomAsync(ChatMessageEvent messageEvent) {
            var userInfo = messageEvent.MessageBody as ChatMessageEvent.UserJoinInfo;

            if ( !activeRooms.ContainsKey(userInfo.ChatRoom.Id) )
                activeRooms.Add(userInfo.ChatRoom.Id, (userInfo.ChatRoom, new List<UserEntity>()));

            var list = activeRooms[userInfo.ChatRoom.Id].ActiveUserLists;

            if ( list.Contains(userInfo.User) )
                throw new AlreadyMemberOfChatException(userInfo.User, userInfo.ChatRoom);

            list.Add(userInfo.User);

            await this.messenger
                .SendMessageToAsync(
                    list.Select(user => user.Id.ToString()),
                    "ChatMessageEvent",
                    messageEvent);
        }

        public async Task QuitFromRoomAsync(ChatMessageEvent messageEvent) {
            var quitUserInfo = messageEvent.MessageBody as ChatMessageEvent.UserQuitInfo;

            // if the quit is not controlled and a room is unknown
            if ( quitUserInfo.UserQuitReason == UserQuitInfo.QuitReason.ConnectionLost ) {
                // find all chat rooms, that the user has membership
                var userChatRoomsMembership = activeRooms
                    .Where(r => r.Value.ActiveUserLists
                        .Contains(quitUserInfo.User))
                    .Select(r => r.Value.ActiveUserLists)
                    .ToList();

                // remove the user from each of lists
                foreach ( var item in userChatRoomsMembership ) {
                    item.Remove(quitUserInfo.User);
                    await this.messenger
                        .SendMessageToAsync(
                            item.Select(user => user.Id.ToString()),
                            "ChatMessageEvent",
                            messageEvent);
                }
                return;
            }

            // if room not found
            if ( !activeRooms.ContainsKey(quitUserInfo.ChatRoomId.Value) )
                throw new NotFoundException(quitUserInfo.ChatRoomId.ToString(), nameof(ChatRoomEntity));

            var list = activeRooms[quitUserInfo.ChatRoomId.Value].ActiveUserLists;
            if ( !list.Contains(quitUserInfo.User) )
                throw new NotMemberOfChatException(quitUserInfo.User, quitUserInfo.ChatRoom);

            list.Remove(quitUserInfo.User);

            await this.messenger
                .SendMessageToAsync(
                    list.Select(user => user.Id.ToString()),
                    "ChatMessageEvent",
                    messageEvent);
        }

        public async Task SendMessageAsync(ChatMessageEvent messageEvent) {
            var sendMessageInfo = messageEvent.MessageBody as ChatMessageEvent.SendMessageInfo;

            // if room not found
            if ( !activeRooms.ContainsKey(sendMessageInfo.ChatRoomId) )
                throw new NotFoundException(sendMessageInfo.ChatRoomId.ToString(), nameof(ChatRoomEntity));

            var list = activeRooms[sendMessageInfo.ChatRoomId].ActiveUserLists;

            if ( !list.Contains(sendMessageInfo.User) )
                throw new NotMemberOfChatException(sendMessageInfo.User, sendMessageInfo.ChatRoom);

            await this.messenger
                .SendMessageToAsync(
                    list.Select(user => user.Id.ToString()),
                    "ChatMessageEvent",
                    messageEvent);
        }
    }
}
