using System.ComponentModel.DataAnnotations;
using EX.Common.Core;
using EX.Data.Enums;

namespace EX.Web.Models
{
    public class ScheduleViewModel : BaseEntity
    {
        [Required]
        [Display(Name = "Дата начала")]
        public string StartDate { get; set; }

        [Required]
        [Display(Name = "Регион")]
        public int? RegionId { get; set; }

        public string RegionName { get; set; }

        [Display(Name = "Код")]
        public int ExamId { get; set; }

        [Display(Name = "Экзамен")]
        public string ExamTitle { get; set; }

        [Required]
        [Range(1, 100)]
        [Display(Name = "Макс. кол-во участников")]
        public int? LimitOfParticipants { get; set; }

        [Display(Name = "Режим")]
        [EnumDataType(typeof(ExamMode))]
        public ExamMode Mode { get; set; }

        [Display(Name = "Статус")]
        [EnumDataType(typeof(ScheduleStatus))]
        public ScheduleStatus Status { get; set; }

    }
}