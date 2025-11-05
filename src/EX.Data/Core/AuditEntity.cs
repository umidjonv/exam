using System;
using System.ComponentModel.DataAnnotations;
using EX.Common.Core;

namespace EX.Data.Core
{
    public abstract class AuditEntity : BaseEntity
    {

        [ScaffoldColumn(false)]
        public bool IsDeleted { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(50)]
        public string CreatedIp { get; set; }

        [ScaffoldColumn(false)]
        public DateTime ModifiedDate { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(50)]
        public string ModifiedIp { get; set; }

    }
}
