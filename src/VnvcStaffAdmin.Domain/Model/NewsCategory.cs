using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using VnvcStaffAdmin.Domain.Attributes;
using VnvcStaffAdmin.Domain.Constants;

namespace VnvcStaffAdmin.Domain.Model
{
    [BsonIgnoreExtraElements]
    [BsonCollection(VnvcStaffCollection.NewsCategory)]
    public class NewsCategory : BaseSoftDeleteEntity
    {
        [StringLength(256)]
        public string? Title { get; set; }

        public string? Avatar { get; set; }
        public int? Order { get; set; }
    }
}