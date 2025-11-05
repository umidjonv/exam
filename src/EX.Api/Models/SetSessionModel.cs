using System.ComponentModel.DataAnnotations;

namespace EX.Api.Models
{
    public class SetSessionModel
    {

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

    }
}