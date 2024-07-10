using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using SimpleChatApplication.DAL.Data.Contexts;
using SimpleChatApplication.DAL.Interfaces;
using System.Linq.Expressions;

namespace SimpleChatApplication.DAL.Data.Repositories {
    public class GenericRepository<EntityType, PrimaryKeyType> : IRepository<EntityType, PrimaryKeyType> where EntityType : class, IEntity {
        private readonly ChatApplicationDbContext context;
        private readonly DbSet<EntityType> dbSet;

        public GenericRepository(ChatApplicationDbContext context) {
            this.context = context;
            dbSet = context.Set<EntityType>();
        }

        public virtual async Task DeleteAsync(PrimaryKeyType id) {
            var entity = await GetByIdAsync(id);
            if ( entity is null )
                throw new NotFoundException(id?.ToString() ?? "", typeof(EntityType).FullName ?? "");

            await DeleteAsync(entity);
        }

        public virtual async Task DeleteAsync(EntityType entity) {
            dbSet.Remove(entity);
        }

        public virtual async Task<IEnumerable<EntityType>> GetAsync(
            Expression<Func<EntityType, bool>>? filter = null,
            Func<IQueryable<EntityType>, IOrderedQueryable<EntityType>>? orderBy = null,
            string includeProperties = "") {
            IQueryable<EntityType> query = dbSet;

            if ( filter != null ) {
                query = query.Where(filter);
            }

            foreach ( var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ) {
                query = query.Include(includeProperty);
            }

            if ( orderBy != null ) {
                return await orderBy(query).ToListAsync();
            }
            else {
                return await query.ToListAsync();
            }
        }

        public virtual async Task<EntityType> GetByIdAsync(PrimaryKeyType id) {
            var entity = await dbSet.FindAsync(id);
            if ( entity is null )
                throw new NotFoundException(id?.ToString() ?? "", typeof(EntityType).FullName ?? "");
            return entity;
        }

        public virtual async Task InsertAsync(EntityType entity) {
            dbSet.Add(entity);
        }

        public virtual async Task UpdateAsync(EntityType entity) {
            dbSet.Update(entity);
        }
    }
}
