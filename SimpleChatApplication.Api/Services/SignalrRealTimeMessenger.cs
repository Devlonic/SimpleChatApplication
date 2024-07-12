using Microsoft.AspNetCore.SignalR;
using SimpleChatApplication.Api.Hubs;
using SimpleChatApplication.BLL.Services;

namespace SimpleChatApplication.Api.Services {
    public class SignalrRealTimeMessenger : IRealTimeMessenger {
        private readonly IHubContext<ChatHub> hubContext;

        public SignalrRealTimeMessenger(IHubContext<ChatHub> hubContext) {
            this.hubContext = hubContext;
        }
        public async Task SendMessageToAsync(IEnumerable<string> users, string endpoint, object message) {
            await hubContext.Clients
                .Users(users)
                .SendAsync(endpoint, message);
        }

        public async Task SendMessageToAsync(string user, string endpoint, object message) {
            await hubContext.Clients
                .User(user)
                .SendAsync(endpoint, message);
        }
    }
}
