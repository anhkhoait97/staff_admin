using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Reflection;
using VnvcStaffAdmin.Domain.Attributes;
using VnvcStaffAdmin.Infrastructure.Interface;
using VnvcStaffAdmin.Infrastructure.Interface.DbContext;
using VnvcStaffAdmin.Infrastructure.Interface.Uow;

namespace VnvcStaffAdmin.Infrastructure.Uow
{
    public abstract class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IMongoContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(IMongoContext context, IServiceProvider serviceProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _repositories = [];
        }

        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories.TryGetValue(typeof(TEntity), out var repository))
            {
                return (IBaseRepository<TEntity>)repository;
            }

            var repositoryInstance = new BaseRepository<TEntity>(_context);
 
            _repositories[typeof(TEntity)] = repositoryInstance;

            return repositoryInstance;
        }


        public async Task<int> CommitAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while committing changes.", ex);
            }
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : class
        {
            var collectionName = typeof(TEntity)
            .GetCustomAttribute<BsonCollectionAttribute>()?
            .CollectionName ?? typeof(TEntity).Name;

            return _context.GetCollection<TEntity>(collectionName);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}