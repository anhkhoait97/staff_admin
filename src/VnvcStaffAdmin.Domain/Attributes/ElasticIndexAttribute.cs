namespace VnvcStaffAdmin.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ElasticIndexAttribute : Attribute
    {
        public string IndexName { get; }

        public ElasticIndexAttribute(string indexName)
        {
            IndexName = indexName;
        }
    }
}