using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EX.Data.Core;

namespace EX.Data.Entities
{
    public class Category : AuditEntity
    {

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public virtual List<Question> Questions { get; set; } = new List<Question>();

        public virtual List<ExamOption> Options { get; set; } = new List<ExamOption>();

    }
}
