using System.ComponentModel.DataAnnotations;
using EX.Data.Core;

namespace EX.Data.Entities
{
    public class Document : AuditEntity
    {

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(8000)]
        public string Description { get; set; }

        public string FileName { get; set; }

    }
}