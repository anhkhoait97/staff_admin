namespace VnvcStaffAdmin.Domain.Model
{
    public class DatasourceResult<T>
    {
        public int? From { get; set; }
        public int? Size { get; set; }
        public long? Total { get; set; }
        public List<T>? Data { get; set; }
        public Dictionary<string, double?>? AggsResult { get; set; } = [];
        public string? Message { get; set; }
    }
}