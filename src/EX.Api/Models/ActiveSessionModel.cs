using System.ComponentModel.DataAnnotations;

namespace EX.Api.Models
{
    public class ActiveSessionModel
    {

        [Required]
        [StringLength(250)]
        public string VideoDriver { get; set; }

        [Required]
        [StringLength(250)]
        public string AudioDriver { get; set; }

    }
}