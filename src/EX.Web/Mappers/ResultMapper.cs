using System.Collections.Generic;
using AutoMapper;
using EX.Data.Entities;
using EX.Web.Mappers.Profile;
using EX.Web.Models;
using X.PagedList;

namespace EX.Web.Mappers
{
    public static class ResultMapper
    {
        private static readonly IMapper Mapper;

        static ResultMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ResultProfile>())
                .CreateMapper();
        }

        public static IPagedList<ExamResultViewModel> ToPager(this IPagedList<ExamResult> items)
        {
            return Mapper.Map<IPagedList<ExamResultViewModel>>(items);
        }

        public static IEnumerable<ExamResultViewModel> ToCollection(this IEnumerable<ExamResult> items)
        {
            return Mapper.Map<IEnumerable<ExamResultViewModel>>(items);
        }

        public static ExamResult ToEntity(this ExamResultViewModel model)
        {
            return Mapper.Map<ExamResult>(model);
        }

        public static ExamResultViewModel ToModel(this ExamResult model)
        {
            return Mapper.Map<ExamResultViewModel>(model);
        }
    }
}
