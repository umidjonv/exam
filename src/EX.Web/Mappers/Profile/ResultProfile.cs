using AutoMapper;
using EX.Data.Entities;
using EX.Web.Mappers.Converter;
using EX.Web.Models;
using X.PagedList;

namespace EX.Web.Mappers.Profile
{
    public class ResultProfile : AutoMapper.Profile
    {
        public ResultProfile()
        {
            CreateMap<ExamResult, ExamResultViewModel>(MemberList.Destination)
                .ReverseMap();
             
            CreateMap<IPagedList<ExamResult>, IPagedList<ExamResultViewModel>>()
                .ConvertUsing<PagedListConverter<ExamResult, ExamResultViewModel>>();
        }
    }
}
