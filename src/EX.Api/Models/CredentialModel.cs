using System.ComponentModel.DataAnnotations;

namespace EX.Api.Models
{
    public class CredentialModel
    {

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

    }
}
