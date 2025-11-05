using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EX.Common.Core;

namespace EX.Web.Models
{
    public class QuestionViewModel : BaseEntity
    {

        [Required]
        [Display(Name = "Категория")]
        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }

        [Display(Name = "Балл")]
        public int? Weight { get; set; }

        [Display(Name = "Базовый")]
        public bool IsBase { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required]
        public List<QuestionLocaleViewModel> Locales { get; set; } = new List<QuestionLocaleViewModel>();

    }
}