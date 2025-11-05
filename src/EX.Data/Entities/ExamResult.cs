using EX.Common.Core;
using EX.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EX.Data.Entities
{
    public class ExamResult : BaseEntity
    {

        public int ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        [Required]
        public int ScheduleId { get; set; }

        public virtual Schedule Schedule { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public ResultStatus Status { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? FinishTime { get; set; }

        public bool? IsAccepted { get; set; }

        public string StatusReason { get; set; }

        public DateTime? StatusDate { set; get; }

        public string ErrorMessage { set; get; }

        public virtual List<AnswerUnit> Units { get; set; } = new List<AnswerUnit>();

        public string QuestionIds { get; set; }

    }
}
