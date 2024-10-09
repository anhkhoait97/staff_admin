using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnvcStaffAdmin.Domain.Model
{
    public class News : BaseEntity
    {
        public string? Title { get; set; }

        public int Order { get; set; }

        public string? Avatar { get; set; }

        public string? Description { get; set; }

        public string? Content { get; set; }

        public string? Type { get; set; }

        public bool IsHotNews { get; set; }

        public bool IsActive { get; set; } = true;

        public List<string> NewsCategoryIds { get; set; } = new List<string>();

        public List<string> Group { get; set; } = new List<string>();

        public string? ObjectApply { get; set; }

        public string? NewsCategoryJoinIds { get; set; }

        public string? ObjectId { get; set; }

        public string? Source { get; set; }

        public string? VideoLink { get; set; }

        public List<DocNewsV2> Docs { get; set; } = new List<DocNewsV2>();

        public bool IsSendNoti { get; set; }

        public List<string> UserSendNotis { get; set; } = new List<string>();

        public ViewAdviseDataDetail ViewAdviseData { get; set; }

        public List<RequestSupportNews> RequestSupports { get; set; } = new List<RequestSupportNews>();

        public List<string> NewsRelateds { get; set; } = new List<string>();

        public List<News>? DisplayNewsRelateds { get; set; }
    }

    public class ViewAdviseDataDetail
    {
        public string? ButtonName { get; set; }
        public string? ButtonStyle { get; set; }
    }

    public class DocNewsV2
    {
        public string? Url { get; set; }

        public string? Type { get; set; }

        public long Size { get; set; }

        public string? NameFile { get; set; }
    }

    public class RequestSupportNews
    {
        public string? Content { get; set; }
        public string? AccountId { get; set; }
        public string? Phone { get; set; }
        public string? DateSubmit { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
