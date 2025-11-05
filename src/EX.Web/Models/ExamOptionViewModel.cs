using System.ComponentModel.DataAnnotations;
using EX.Common.Core;

namespace EX.Web.Models
{
    public class ExamOptionViewModel : BaseEntity
    {
          
        [Required]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        [Required]
        public int? CountOfRegularQuestions { get; set; }

        [Required]
        public int? CountOfBaseQuestions { get; set; }

    }
}