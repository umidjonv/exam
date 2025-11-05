using System.ComponentModel.DataAnnotations;
using EX.Common.Core;

namespace EX.Web.Models
{
    public class QuestionAnswerViewModel : BaseEntity
    {

        [Required]
        [StringLength(1000)]
        public string Title { get; set; }
         
        public bool IsCorrect { get; set; }

        public int Weight { get; set; }

    }
}