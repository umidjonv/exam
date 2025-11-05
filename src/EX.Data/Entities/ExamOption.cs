using System.ComponentModel.DataAnnotations;
using EX.Data.Core;

namespace EX.Data.Entities
{
    public class ExamOption : AuditEntity
    {

        [Required]
        public int ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        [Required]
        public int CategoryId { get; set; }
         
        public virtual Category Category { get; set; }

        [Required]
        public int CountOfRegularQuestions { get; set; }

        [Required]
        public int CountOfBaseQuestions { get; set; } 

    }
}