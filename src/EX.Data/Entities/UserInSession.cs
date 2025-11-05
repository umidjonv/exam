using System;
using System.ComponentModel.DataAnnotations;
using EX.Common.Core;

namespace EX.Data.Entities
{
    public class UserInSession : BaseEntity
    {

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public DateTime LoginTime { get; set; }

        [StringLength(50)]
        public string IpAddress { get; set; }

        [StringLength(250)]
        public string UserAgent { get; set; }

    }
}