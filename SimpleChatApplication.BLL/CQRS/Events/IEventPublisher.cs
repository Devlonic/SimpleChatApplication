namespace SimpleChatApplication.BLL.CQRS.Events {
    public interface IEventPublisher<EventDataType> {
        Task SendEventAsync(EventDataType data);
    }
}
