using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using VnvcStaffAdmin.Domain.Constants;

namespace VnvcStaffAdmin.Identity.Models
{
    [CollectionName(VnvcStaffCollection.Role)]
    public class ApplicationRole : MongoIdentityRole<string>
    {
    }
}