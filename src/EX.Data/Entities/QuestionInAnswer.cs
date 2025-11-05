using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EX.Common.Core;

namespace EX.Data.Entities
{
    public class QuestionInAnswer : BaseEntity
    {

        [Required]
        [StringLength(1000)]
        public string Title { get; set; }

        [Required]
        public int QuestionLocaleId { get; set; }

        public virtual QuestionLocale QuestionLocale { get; set; }

        public virtual List<QuestionInCorrect> Corrects { get; set; } = new List<QuestionInCorrect>();

        public virtual List<AnswerUnit> Units { get; set; } = new List<AnswerUnit>();

        public char? Order { get; set; }

    }
}