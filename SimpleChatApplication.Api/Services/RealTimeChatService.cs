using Ardalis.GuardClauses;
using Microsoft.AspNetCore.SignalR;
using SimpleChatApplication.Api.Hubs;
using SimpleChatApplication.BLL.Exceptions;
using SimpleChatApplication.BLL.Models.EventTypes;
using SimpleChatApplication.BLL.Services;
using SimpleChatApplication.DAL.Entities;

namespace SimpleChatApplication.Api.Services {
    public class RealTimeChatService : IChatService {
        private readonly Dictionary<int, (ChatRoomEntity ChatRoom, IList<UserEntity> ActiveUserLists)> activeRooms = new();
        private readonly IHubContext<ChatHub> hubContext;

        public RealTimeChatService(IHubContext<ChatHub> hubContext) {
            this.hubContext = hubContext;
        }

        public async Task JoinToRoomAsync(ChatMessageEvent messageEvent) {
            var userInfo = messageEvent.MessageBody as ChatMessageEvent.UserJoinInfo;

            if ( !activeRooms.ContainsKey(userInfo.ChatRoom.Id) )
                activeRooms.Add(userInfo.ChatRoom.Id, (userInfo.ChatRoom, new List<UserEntity>()));

            var list = activeRooms[userInfo.ChatRoom.Id].ActiveUserLists;

            if ( list.Contains(userInfo.User) )
                throw new AlreadyMemberOfChatException(userInfo.User, userInfo.ChatRoom);

            list.Add(userInfo.User);

            await this.hubContext.Clients
                .Users(list.Select(user => user.Id.ToString()))
                .SendAsync("ChatMessageEvent", messageEvent);
        }

        public async Task QuitFromRoomAsync(ChatMessageEvent messageEvent) {
            throw new NotImplementedException();
        }

        public async Task SendMessageAsync(ChatMessageEvent messageEvent) {
            var sendMessageInfo = messageEvent.MessageBody as ChatMessageEvent.SendMessageInfo;

            // if room not found
            if ( !activeRooms.ContainsKey(sendMessageInfo.ChatRoomId) )
                throw new NotFoundException(sendMessageInfo.ChatRoomId.ToString(), nameof(ChatRoomEntity));

            var list = activeRooms[sendMessageInfo.ChatRoomId].ActiveUserLists;

            if ( !list.Contains(sendMessageInfo.User) )
                throw new NotMemberOfChatException(sendMessageInfo.User, sendMessageInfo.ChatRoom);

            await this.hubContext.Clients
                .Users(list.Select(user => user.Id.ToString()))
                .SendAsync("ChatMessageEvent", messageEvent);
        }
    }
}
