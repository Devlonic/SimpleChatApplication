﻿
namespace SimpleChatApplication.DAL.Interfaces {
    public interface IUnitOfWork : IDisposable {
        Task CommitAsync();
        void Rollback();
        IRepository<EntityType, int> GetRepository<EntityType>() where EntityType : class, IEntity;
    }
}
