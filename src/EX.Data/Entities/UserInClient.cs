using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EX.Common.Core;

namespace EX.Data.Entities
{
    public class UserInClient : BaseEntity
    {

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public DateTime LoginTime { get; set; }

        [Required]
        [StringLength(50)]
        public string UserSession { get; set; }

        [Required]
        [StringLength(50)]
        public string IpAddress { get; set; }

        [StringLength(50)]
        public string ComputerDomain { get; set; }

        [Required]
        [StringLength(50)]
        public string ComputerName { get; set; }

        [Required]
        [StringLength(50)]
        public string ComputerUser { get; set; }

        [StringLength(250)]
        public string VideoDriver { get; set; }

        [StringLength(250)]
        public string AudioDriver { get; set; }
         
        public bool? IsConnected { get; set; }

        public DateTime? ConnectTime { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime? DisconnectTime { get; set; }

        public virtual List<UserInExams> Exams { get; set; } = new List<UserInExams>();

    }
}