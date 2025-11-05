using System.ComponentModel.DataAnnotations;

namespace EX.Data.Enums
{
    public enum ScheduleStatus
    {

        [Display(Name = "Новый")]
        New,

        [Display(Name = "В ожидании")]
        Pending,

        [Display(Name = "Активный")]
        Active,

        [Display(Name = "Законченный")]
        Finished

    }
}