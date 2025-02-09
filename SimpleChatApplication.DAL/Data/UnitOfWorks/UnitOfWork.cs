﻿using SimpleChatApplication.DAL.Data.Contexts;
using SimpleChatApplication.DAL.Data.Repositories;
using SimpleChatApplication.DAL.Entities;
using SimpleChatApplication.DAL.Interfaces;
using System.Collections.Concurrent;

namespace SimpleChatApplication.DAL.Data.UnitOfWorks {
    public class UnitOfWork : IUnitOfWork {
        private readonly ChatApplicationDbContext dbContext;

        // a vault for all repositories
        private readonly ConcurrentDictionary<Type, object> repositories = new();
        public UnitOfWork(ChatApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }

        public async Task CommitAsync() {
            try {
                await this.dbContext.SaveChangesAsync();
            }
            catch ( Exception ) {
                Rollback();
                throw;
            }
            finally {
            }
        }

        public void Rollback() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get repository instance, using generic type
        /// </summary>
        /// <typeparam name="EntityType"></typeparam>
        /// <returns>A repository to manipulate storage of provided type</returns>
        IRepository<EntityType, int> IUnitOfWork.GetRepository<EntityType>() where EntityType : class {
            return (IRepository<EntityType, int>)repositories.GetOrAdd(typeof(EntityType), _ => new GenericRepository<EntityType, int>(dbContext));
        }
    }
}
