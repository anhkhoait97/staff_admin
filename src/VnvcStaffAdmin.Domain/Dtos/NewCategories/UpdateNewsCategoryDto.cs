using System.ComponentModel.DataAnnotations;
using VnvcStaffAdmin.Domain.Interface;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.Domain.Dtos.NewCategories
{
    public class UpdateNewsCategoryDto : NewsCategory, IUpdateColumn
    {
        [Required]
        public List<string>? Columns { get; set; } = [];
    }
}