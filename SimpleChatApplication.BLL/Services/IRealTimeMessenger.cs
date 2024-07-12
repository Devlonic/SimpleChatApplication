namespace SimpleChatApplication.BLL.Services {
    public interface IRealTimeMessenger {
        Task SendMessageToAsync(IEnumerable<string> users, string endpoint, object message);
        Task SendMessageToAsync(string user, string endpoint, object message);
    }
}
