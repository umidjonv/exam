using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EX.Common.Core;

namespace EX.Data.Entities
{
    public class User : BaseEntity
    {

        [Required]
        [StringLength(50)]
        public new string Id { get; set; }

        [Required]
        public DateTime LastActivity { get; set; }

        public virtual List<UserInExams> Exams { get; set; } = new List<UserInExams>();

        public virtual List<UserInSession> Sessions { get; set; } = new List<UserInSession>();

        public virtual List<UserInClient> Clients { get; set; } = new List<UserInClient>();

        public virtual List<ExamResult> Results { get; set; } = new List<ExamResult>();

    }
}
