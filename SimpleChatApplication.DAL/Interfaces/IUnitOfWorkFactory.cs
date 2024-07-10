namespace SimpleChatApplication.DAL.Interfaces {
    public interface IUnitOfWorkFactory {
        IUnitOfWork CreateUnitOfWork();
    }
}
