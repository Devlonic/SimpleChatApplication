using Microsoft.AspNetCore.SignalR;
using SimpleChatApplication.Api.Hubs;
using SimpleChatApplication.BLL.CQRS.Events;
using SimpleChatApplication.BLL.Models.EventTypes;
using SimpleChatApplication.DAL.Data.Contexts;

namespace SimpleChatApplication.Api.Events {
    public class ChatMessageEventPublisher : IEventPublisher<ChatMessageEvent> {
        private readonly IHubContext<ChatHub> hub;
        private readonly ChatApplicationDbContext dbContext;

        public ChatMessageEventPublisher(ChatApplicationDbContext db, IHubContext<ChatHub> hub) {
            dbContext = db;
            this.hub = hub;
        }
        public async Task SendEventAsync(ChatMessageEvent data) {
            switch ( data.MessageType ) {
                case ChatMessageEvent.ChatMessageType.UserJoin:
                var parameters = data.MessageBody as ChatMessageEvent.UserJoinInfo;
                await this.hub.Clients.User(parameters.UserId.ToString()).SendAsync("ChatMessageEvent", data);
                break;
                case ChatMessageEvent.ChatMessageType.UserExit:
                break;
                case ChatMessageEvent.ChatMessageType.NewMessageReceived:
                break;
                case ChatMessageEvent.ChatMessageType.ChatDeleted:
                break;
                default:
                break;
            }
        }
    }
}
