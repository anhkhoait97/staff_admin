using MongoDB.Driver;
using System.Reflection;
using VnvcStaffAdmin.Domain.Attributes;
using VnvcStaffAdmin.Infrastructure.Interface.DbContext;

namespace VnvcStaffAdmin.Infrastructure.Context
{
    public class VnvcStaffContext : IVnvcStaffContext, IDisposable
    {
        private IMongoDatabase _database;
        private MongoClient _mongoClient;
        private readonly List<Func<Task>> _commands;

        public VnvcStaffContext()
        {
            if (_mongoClient != null) return;

            var connectionString = Environment.GetEnvironmentVariable("MONGO_ADMINSTAFF_CONNECTION_STRING");
            var databaseName = Environment.GetEnvironmentVariable("ADMIN_STAFF_MONGODB_DATABASE_NAME");

            _mongoClient = new MongoClient(connectionString);
            _database = _mongoClient.GetDatabase(databaseName);

            _commands = [];
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            var collectionName = typeof(T)
            .GetCustomAttribute<BsonCollectionAttribute>()?
            .CollectionName ?? typeof(T).Name;

            return _database.GetCollection<T>(collectionName);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                var commandTasks = _commands.Select(c => c());
                await Task.WhenAll(commandTasks);
                return _commands.Count;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving changes.", ex);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}