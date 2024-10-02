using MongoDB.Driver;

namespace VnvcStaffAdmin.Infrastructure.Interface.DbContext
{
    public interface IMongoContext : IDisposable
    {
        void AddCommand(Func<Task> func);

        Task<int> SaveChangesAsync();

        IMongoCollection<T> GetCollection<T>(string name);
    }
}