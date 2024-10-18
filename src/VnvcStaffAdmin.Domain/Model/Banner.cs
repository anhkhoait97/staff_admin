using MongoDB.Bson.Serialization.Attributes;
using VnvcStaffAdmin.Domain.Attributes;
using VnvcStaffAdmin.Domain.Constants;

namespace VnvcStaffAdmin.Domain.Model
{
    [BsonIgnoreExtraElements]
    [BsonCollection(VnvcStaffCollection.Banner)]
    public class Banner : BaseSoftDeleteEntity
    {
        public string? Name { get; set; }

        public int Order { get; set; }

        public string? Description { get; set; }

        public string? Link { get; set; }

        public string? ImageUrl { get; set; }

        public string? Type { get; set; }

        public bool IsActive { get; set; } = true;
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? StartDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? EndDate { get; set; }

        public string? Ratio { get; set; }

        public string? VideoLink { get; set; }

        public ViewConfigBanner? Config { get; set; }
    }

    public class ViewConfigBanner
    {
        /// <summary>
        /// dữ liệu => dieuhuongtrenapp | xemtintuc | video | chitietvaccine
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// điều hướng đến tính năng
        /// </summary>
        public string? RedirectApp { get; set; }

        /// <summary>
        /// loại tin tức => tronghethong | ngoaihethong
        /// </summary>
        public string? TypeNews { get; set; }

        /// <summary>
        /// Relation với table NewsV2
        /// </summary>
        public string? NewsId { get; set; }

        /// <summary>
        /// có giá trị khi TypeNews là ngoaihethong
        /// </summary>
        public string? NewsLink { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? VideoLink { get; set; }

        /// <summary>
        /// Relation với table VaccineCategories
        /// </summary>
        public string? VaccineId { get; set; }
    }
}