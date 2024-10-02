using MongoDB.Driver;

namespace VnvcStaffAdmin.Infrastructure.Interface.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        Task<int> CommitAsync();

        IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : class;
    }
}