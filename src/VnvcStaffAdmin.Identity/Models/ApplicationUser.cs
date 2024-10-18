using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System.ComponentModel.DataAnnotations;
using VnvcStaffAdmin.Domain.Attributes;
using VnvcStaffAdmin.Domain.Constants;
using VnvcStaffAdmin.Domain.Interface;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Identity.Models
{
    [CollectionName(VnvcStaffCollection.ApplicationUser)]
    [BsonCollection(VnvcStaffCollection.ApplicationUser)]
    public class ApplicationUser : MongoIdentityUser<string>, IAuditable, ISoftDeletable
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Gender { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? Birthday { get; set; }
        public bool IsActive { get; set; }
        public string? AvatarUrl { get; set; }
        public bool IsDelete { get; set; } = false;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<string> RoleNames { get; set; } = [];
    }
}