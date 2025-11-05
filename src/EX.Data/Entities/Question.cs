using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EX.Data.Core;

namespace EX.Data.Entities
{
    public class Question : AuditEntity
    {

        [Required]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
         
        public int? Weight { get; set; }

        [Required]
        public bool IsBase { get; set; }

        public virtual List<QuestionLocale> Locales { get; set; } = new List<QuestionLocale>();

        public virtual List<QuestionInCorrect> Corrects { get; set; } = new List<QuestionInCorrect>();

        public virtual List<AnswerUnit> Units { get; set; } = new List<AnswerUnit>();

    }
}
