using AutoMapper;
using EX.Data.Entities;
using EX.Web.Mappers.Converter;
using EX.Web.Models;
using X.PagedList;

namespace EX.Web.Mappers.Profile
{
    public class ExamProfile : AutoMapper.Profile
    {
        public ExamProfile()
        { 
            CreateMap<ExamOption, ExamOptionViewModel>(MemberList.Destination)
                .ForMember(a => a.CategoryName, b => b.MapFrom(s => s.Category.Name))
                .ReverseMap();

            CreateMap<Exam, ExamViewModel>(MemberList.Destination)
                .ReverseMap();

            CreateMap<IPagedList<Exam>, IPagedList<ExamViewModel>>()
                .ConvertUsing<PagedListConverter<Exam, ExamViewModel>>();
        }
    }
}