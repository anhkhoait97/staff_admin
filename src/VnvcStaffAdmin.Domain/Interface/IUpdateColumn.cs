using System.ComponentModel.DataAnnotations;

namespace VnvcStaffAdmin.Domain.Interface
{
    public interface IUpdateColumn
    {
        public List<string> Columns { get; set; }
    }
}