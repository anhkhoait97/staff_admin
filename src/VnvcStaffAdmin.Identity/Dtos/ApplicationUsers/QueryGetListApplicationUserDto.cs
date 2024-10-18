using VnvcStaffAdmin.Domain.Interface;

namespace VnvcStaffAdmin.Identity.Dtos.ApplicationUsers
{
    public class QueryGetListApplicationUserDto : IQueryPaging
    {
        public string? SearchText { get; set; }
        public int From { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}