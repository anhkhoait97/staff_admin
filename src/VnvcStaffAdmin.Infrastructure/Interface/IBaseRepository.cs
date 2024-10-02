using MongoDB.Driver;
using System.Linq.Expressions;

namespace VnvcStaffAdmin.Infrastructure.Interface
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> GetByIdAsync(string id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task AddAsync(TEntity entity);

        Task AddManyAsync(IEnumerable<TEntity> entities);

        void AddManyCommand(IEnumerable<TEntity> entities);

        void AddCommand(TEntity entity);

        Task UpdateAsync(TEntity entity, string id);

        Task<TEntity> UpdateAsync(
            FilterDefinition<TEntity> filter,
            UpdateDefinition<TEntity> update,
            FindOneAndUpdateOptions<TEntity> options);

        void UpdateCommand(
            FilterDefinition<TEntity> filter,
            UpdateDefinition<TEntity> update,
            FindOneAndUpdateOptions<TEntity> options);

        Task SoftDeleteAsync(string id);

        void SoftDeleteCommand(string id);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> FindPagingAsync(Expression<Func<TEntity, bool>> predicate, int skip, int limit, SortDefinition<TEntity>? sort = null);

        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate);
    }
}