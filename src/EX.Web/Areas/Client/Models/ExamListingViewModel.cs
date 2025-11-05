using EX.Data.Entities;
using EX.Data.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace EX.Web.Areas.Client.Models
{
    public class ExamListingViewModel
    {

        public IPagedList<Schedule> Items { get; set; }
           
        public SelectList Regions { get; set; }

        public string UserId { get; set; }

        public ResultStatus ExamProgressStatus { get; set; }

        public int ScheduleId { get; set; }

        public int CurrentPosition { get; set; }

        public string Message { get; set; }

    }
}
