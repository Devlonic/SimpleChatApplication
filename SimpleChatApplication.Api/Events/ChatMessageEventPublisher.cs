using Microsoft.AspNetCore.SignalR;
using SimpleChatApplication.Api.Hubs;
using SimpleChatApplication.BLL.CQRS.Events;
using SimpleChatApplication.BLL.Models.EventTypes;
using SimpleChatApplication.BLL.Services;
using SimpleChatApplication.DAL.Data.Contexts;

namespace SimpleChatApplication.Api.Events {
    // do not use this class, not implemented
    public class ChatMessageEventPublisher : IEventPublisher<ChatMessageEvent> {
        private readonly IChatService chatService;

        public ChatMessageEventPublisher(IChatService chatService) {
            this.chatService = chatService;
        }
        public async Task SendEventAsync(ChatMessageEvent data) {
            throw new NotImplementedException();
            switch ( data.MessageType ) {
                case ChatMessageEvent.ChatMessageType.UserJoin:
                var parameters = data.MessageBody as ChatMessageEvent.UserJoinInfo;
                //await chatService.JoinToRoomAsync(parameters);
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
