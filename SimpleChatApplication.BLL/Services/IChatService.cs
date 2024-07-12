using SimpleChatApplication.BLL.Models.EventTypes;
using SimpleChatApplication.DAL.Entities;

namespace SimpleChatApplication.BLL.Services {
    public interface IChatService {
        Task JoinToRoomAsync(ChatMessageEvent messageEvent);
        Task QuitFromRoomAsync(ChatMessageEvent messageEvent);
        Task SendMessageAsync(ChatMessageEvent messageEvent);
    }
}
