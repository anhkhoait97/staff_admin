namespace VnvcStaffAdmin.Application.ExtendServices.Redis
{
    public interface IRedisService
    {
        Task SetValueAsync(string key, string value, TimeSpan? expiry = null);

        Task<string> GetValueAsync(string key);

        Task<bool> DeleteKeyAsync(string key);

        Task<bool> KeyExistsAsync(string key);
    }
}