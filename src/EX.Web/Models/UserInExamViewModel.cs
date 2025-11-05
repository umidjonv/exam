using System;
using EX.Common.Core;

namespace EX.Web.Models
{
    public class UserInExamViewModel : BaseEntity
    {
        public int UserId { get; set; }

        public int ScheduleId { get; set; }

        public ScheduleViewModel Schedule { get; set; }

        public int ExamId { get; set; }

        public ExamViewModel Exam { get; set; }

        public bool? IsPassed { get; set; }

        public bool IsAdmit { get; set; }

        public DateTime? PassedTime { get; set; }

        public DateTime RequestTime { get; set; }

        public string ErrorMessage { get; set; }
    }
}