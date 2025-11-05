using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EX.Data.Core;

namespace EX.Data.Entities
{
    public class Exam : AuditEntity
    {

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [Required]
        public int DurationInMinutes { get; set; }

        [Required]
        public int PeriodInDays { get; set; }

        [Required]
        public int PassingScore { get; set; }

        [Required]
        public string CultureId { get; set; }

        public virtual List<ExamOption> Options { get; set; } = new List<ExamOption>();

        public virtual List<Schedule> Schedules { get; set; } = new List<Schedule>();

        public virtual List<UserInExams> Users { get; set; } = new List<UserInExams>();

        public virtual List<ExamResult> Results { get; set; } = new List<ExamResult>();

    }
}