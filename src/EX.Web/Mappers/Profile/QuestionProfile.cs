using AutoMapper;
using EX.Data.Entities;
using EX.Web.Mappers.Converter;
using EX.Web.Models;
using X.PagedList;

namespace EX.Web.Mappers.Profile
{
    public class QuestionProfile : AutoMapper.Profile
    {
        public QuestionProfile()
        {

            CreateMap<QuestionAnswerViewModel, QuestionInAnswer>(MemberList.Destination)
                .ReverseMap();

            CreateMap<QuestionLocaleViewModel, QuestionLocale>(MemberList.Destination)
                .ReverseMap();

            CreateMap<Question, QuestionViewModel>(MemberList.Destination)
                .ForMember(a => a.CategoryName, b => b.MapFrom(s => s.Category.Name))
                .ReverseMap();

            CreateMap<IPagedList<Question>, IPagedList<QuestionViewModel>>()
               .ConvertUsing<PagedListConverter<Question, QuestionViewModel>>();
        }
    }
}
