using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EX.Common.Core;

namespace EX.Web.Models
{
    public class ExamViewModel : BaseEntity
    {

        [Display(Name = "Код")]
        public string Code { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Продолжительность экзамена")]
        public int? DurationInMinutes { get; set; }

        public string DurationFormat=> DurationInMinutes > 0
            ? TimeSpan.FromMinutes(DurationInMinutes.Value).ToString(@"hh\:mm")
            : "00:00";

        [Required]
        [Display(Name = "Срок пересдачи")]
        public int? PeriodInDays { get; set; }

        [Required]
        [Display(Name = "Проходной балл")]
        public int? PassingScore { get; set; }

        [Required]
        [Display(Name = "Язык экзамена")]
        public string CultureId { get; set; }

        [Required]
        public virtual List<ExamOptionViewModel> Options { get; set; } = new List<ExamOptionViewModel>();
        
        
    }
}