using System;
using System.ComponentModel.DataAnnotations;
using EX.Common.Core;

namespace EX.Data.Entities
{
    public class UserInExams : BaseEntity
    {

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        public virtual User User { get; set; }
           
        [Required]
        public int ScheduleId { get; set; }

        public virtual Schedule Schedule { get; set; }

        public int ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        public bool? IsPassed { get; set; }

        public bool IsAdmit { get; set; }

        public DateTime? PassedTime { get; set; }

        public DateTime RequestTime { get; set; }

        public int? ClientId { get; set; }

        public virtual UserInClient Client { get; set; }

    }

}