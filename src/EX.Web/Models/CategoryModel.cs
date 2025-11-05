using System.ComponentModel.DataAnnotations;
using EX.Common.Core;

namespace EX.Web.Models
{
    public class CategoryViewModel : BaseEntity
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [StringLength(500)]
        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}