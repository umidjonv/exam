using System.ComponentModel.DataAnnotations;

namespace EX.Data.Enums
{
    public enum ResultStatus
    {
        
        [Display(Name = "Pass")]
        Pass,
        
        [Display(Name = "Active")]
        Active,
        
        [Display(Name = "Failed")]
        Failed,
        
        [Display(Name = "Error")]
        Error,

        [Display(Name = "NotStarted")]
        NotStarted

    }
}
