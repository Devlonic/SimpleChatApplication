using Microsoft.AspNetCore.SignalR;

namespace SimpleChatApplication.Api {
    public class ApplicationUserIdProvider : IUserIdProvider {
        public string? GetUserId(HubConnectionContext connection) {
            // primitive query-based authentication
            return connection.GetHttpContext()?.Request.Query["userId"];
        }
    }
}
