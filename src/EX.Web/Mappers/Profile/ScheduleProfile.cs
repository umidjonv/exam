using AutoMapper;
using EX.Data.Entities;
using EX.Web.Models;

namespace EX.Web.Mappers.Profile
{
    public class ScheduleProfile : AutoMapper.Profile
    {
        public ScheduleProfile()
        {
            CreateMap<Schedule, ScheduleViewModel>(MemberList.Destination)
                .ForMember(a => a.StartDate, b => b.MapFrom(c => c.StartDate.ToString("dd/MM/yyyy HH:mm")))
                .ForMember(a => a.ExamTitle, b => b.MapFrom(c => c.Exam.Title))
                .ReverseMap();
        }
    }
}