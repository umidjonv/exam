using System;
namespace EX.Api.Dtos
{
    public class ExamDto
    {
        public int Id { get; set; }

        public int ScheduleId { get; set; }

        public string Mode{ get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public string Participants { get; set; }

        public string Status { get; set; }

        public bool? IsEnrolled { get; set; }

    }
}