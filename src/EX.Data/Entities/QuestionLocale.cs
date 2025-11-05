using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EX.Common.Core;

namespace EX.Data.Entities
{

    public class QuestionLocale : BaseEntity
    {

        [Required]
        [StringLength(1000)]
        public string Title { get; set; }

        [StringLength(5000)]
        public string Description { get; set; }

        [Required]
        public string CultureId { get; set; }

        public virtual List<QuestionInAnswer> Answers { get; set; } = new List<QuestionInAnswer>();

    }
}
