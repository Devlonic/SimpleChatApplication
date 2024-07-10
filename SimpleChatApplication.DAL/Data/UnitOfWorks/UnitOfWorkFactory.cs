using SimpleChatApplication.DAL.Data.Contexts;
using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.DAL.Data.UnitOfWorks {
    public class UnitOfWorkFactory : IUnitOfWorkFactory {
        private readonly ChatApplicationDbContext dbContext;

        public UnitOfWorkFactory(ChatApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }

        public IUnitOfWork CreateUnitOfWork() {
            return new UnitOfWork(dbContext);
        }
    }
}
