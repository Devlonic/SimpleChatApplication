using System.Linq.Expressions;

namespace SimpleChatApplication.DAL.Interfaces {
    public interface IRepository<EntityType, PrimaryKeyType> where EntityType : IEntity {

        /// <summary>
        /// Get entity by it`s primary key
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="Ardalis.GuardClauses.NotFoundException">Throws when entity with key now found</exception>
        /// <returns>Entity instance</returns>
        Task<EntityType> GetByIdAsync(PrimaryKeyType id);
        Task<IEnumerable<EntityType>> GetAsync(
            Expression<Func<EntityType, bool>>? filter = null,
            Func<IQueryable<EntityType>, IOrderedQueryable<EntityType>>? orderBy = null,
            string includeProperties = "");
        Task<EntityType?> GetFirstByFilter(
            Expression<Func<EntityType, bool>>? filter = null,
            string includeProperties = "");

        Task DeleteAsync(PrimaryKeyType id);
        Task DeleteAsync(EntityType entity);
        Task UpdateAsync(EntityType entity);
        Task InsertAsync(EntityType entity);
    }
}
