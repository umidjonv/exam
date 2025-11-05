using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EX.Common.Core;

namespace EX.Web.Models
{
    public class QuestionLocaleViewModel : BaseEntity
    {

        [StringLength(1000)]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [StringLength(5000)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required]
        public string CultureId { get; set; }

        public string CultureName { get; set; }
         
        public List<QuestionAnswerViewModel> Answers { get; set; } = new List<QuestionAnswerViewModel>();

    }
}