using System.ComponentModel.DataAnnotations;
using EX.Common.Core;

namespace EX.Data.Entities
{
    public class QuestionInCorrect : BaseEntity
    {

        [Required]
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }

        [Required]
        public int AnswerId { get; set; }

        public virtual QuestionInAnswer Answer { get; set; }

        [Required]
        public string CultureId { get; set; }

    }
}