using MongoDB.Driver;
using System.Linq.Expressions;
using System.Reflection;
using VnvcStaffAdmin.Domain.Attributes;
using VnvcStaffAdmin.Domain.Interface;
using VnvcStaffAdmin.Infrastructure.Interface;
using VnvcStaffAdmin.Infrastructure.Interface.DbContext;

namespace VnvcStaffAdmin.Infrastructure
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected IMongoCollection<TEntity> _collection;
        protected readonly IMongoContext _context;

        public BaseRepository(IMongoContext context)
        {
            _context = context;

            var collectionName = typeof(TEntity)
                .GetCustomAttribute<BsonCollectionAttribute>()?.CollectionName
                ?? typeof(TEntity).Name;

            _collection = _context.GetCollection<TEntity>(collectionName);
        }

        public virtual async Task<TEntity> GetByIdAsync(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("Id", id)
                & Builders<TEntity>.Filter.Eq("IsDelete", false);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var filter = Builders<TEntity>.Filter.Eq("IsDelete", false);
            return await _collection.Find(filter).ToListAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            if (entity is IAuditable auditableEntity)
            {
                auditableEntity.CreatedAt = DateTime.UtcNow;
                auditableEntity.CreatedBy = "admin";
            }

            if (entity is ISoftDeletable softDeletableEntity)
            {
                softDeletableEntity.IsDelete = false;
            }

            await _collection.InsertOneAsync(entity);
        }

        public virtual async Task AddManyAsync(IEnumerable<TEntity> entities)
        {
            var userName = "admin";
            foreach (var entity in entities)
            {
                if (entity is IAuditable auditableEntity)
                {
                    auditableEntity.CreatedAt = DateTime.UtcNow;
                    auditableEntity.CreatedBy = userName;
                }

                if (entity is ISoftDeletable softDeletableEntity)
                {
                    softDeletableEntity.IsDelete = false;
                }
            }

            await _collection.InsertManyAsync(entities);
        }

        public virtual void AddManyCommand(IEnumerable<TEntity> entities)
        {
            _context.AddCommand(async () => await AddManyAsync(entities));
        }

        public virtual void AddCommand(TEntity entity)
        {
            _context.AddCommand(async () => await AddAsync(entity));
        }

        public virtual async Task<TEntity> UpdateAsync(
            FilterDefinition<TEntity> filter,
            UpdateDefinition<TEntity> update,
            FindOneAndUpdateOptions<TEntity>? options = null)
        {
            if (typeof(IAuditable).IsAssignableFrom(typeof(TEntity)))
            {
                update = Builders<TEntity>.Update.Combine(
                    update,
                    Builders<TEntity>.Update.Set("UpdatedAt", DateTime.UtcNow),
                    Builders<TEntity>.Update.Set("UpdatedBy", "admin")
                );
            }

            return await _collection.FindOneAndUpdateAsync(filter, update, options);
        }

        public virtual async Task UpdateAsync(TEntity entity, string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);

            if (entity is IAuditable auditableEntity)
            {
                auditableEntity.UpdatedAt = DateTime.UtcNow;
                auditableEntity.UpdatedBy = "admin";
            }

            await _collection.ReplaceOneAsync(filter, entity);
        }

        public virtual void UpdateCommand(
            FilterDefinition<TEntity> filter,
            UpdateDefinition<TEntity> update,
            FindOneAndUpdateOptions<TEntity>? options = null)
        {
            _context.AddCommand(async () => await UpdateAsync(filter, update, options));
        }

        public virtual async Task SoftDeleteAsync(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);

            var update = Builders<TEntity>.Update.Set("IsDelete", true)
                                                 .Set("UpdatedAt", DateTime.UtcNow)
                                                 .Set("UpdatedBy", "admin");

            await _collection.UpdateOneAsync(filter, update);
        }

        public virtual void SoftDeleteCommand(string id)
        {
            _context.AddCommand(() => SoftDeleteAsync(id));
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _collection.Find(predicate).FirstOrDefaultAsync();
        }

        public virtual void Dispose()
        {
            _context?.Dispose();
        }

        public virtual async Task<IEnumerable<TEntity>> FindPagingAsync(Expression<Func<TEntity, bool>> predicate, int skip, int limit, SortDefinition<TEntity>? sort = null)
        {
            return await _collection.Find(predicate).Sort(sort).Skip(skip).Limit(limit).ToListAsync();
        }

        public virtual async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _collection.Find(predicate).CountDocumentsAsync();
        }
    }
}