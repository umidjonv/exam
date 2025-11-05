using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EX.Common.Core;
using EX.Data.Entities;
using EX.Data.Enums;

namespace EX.Web.Models
{
    public class ExamResultViewModel : BaseEntity
    {
        public int ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        [Required]
        public int ScheduleId { get; set; }

        public virtual Schedule Schedule { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public ResultStatus FinishedStatus { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? FinishTime { get; set; }

        public bool? IsAccepted { get; set; }

        public string StatusReason { get; set; }

        public DateTime? StatusDate { set; get; }

        public virtual List<AnswerUnit> Units { get; set; } = new List<AnswerUnit>();

        public string QuestionIds { get; set; }
    }
}
