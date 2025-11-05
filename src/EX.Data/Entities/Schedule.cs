using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EX.Data.Core;
using EX.Data.Enums;

namespace EX.Data.Entities
{
    public class Schedule : AuditEntity
    {

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public int RegionId { get; set; }

        [Required]
        public int ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        [Required]
        public int LimitOfParticipants { get; set; }

        [Required]
        public ExamMode Mode { get; set; }

        [Required]
        public ScheduleStatus Status { get; set; }

        public virtual List<UserInExams> Users { get; set; } = new List<UserInExams>();
        
        public virtual List<ExamResult> Results { get; set; } = new List<ExamResult>();

    }
}