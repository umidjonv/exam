using System.Collections.Generic;
using AutoMapper;
using EX.Data.Entities;
using EX.Web.Mappers.Profile;
using EX.Web.Models;
using X.PagedList;

namespace EX.Web.Mappers
{
    public static class UserInExamsMapper
    {

        private static readonly IMapper Mapper;

        static UserInExamsMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<UserInExamProfile>())
                .CreateMapper();
        }

        public static IPagedList<UserInExamViewModel> ToPager(this IPagedList<UserInExams> items)
        {
            return Mapper.Map<IPagedList<UserInExamViewModel>>(items);
        }

        public static UserInExams ToEntity(this UserInExamViewModel model)
        {
            return Mapper.Map<UserInExams>(model);
        }

        public static UserInExamViewModel ToModel(this UserInExams model)
        {
            return Mapper.Map<UserInExamViewModel>(model);
        }

        public static IEnumerable<UserInExamViewModel> ToCollection(this IEnumerable<UserInExams> items)
        {
            return Mapper.Map<IEnumerable<UserInExamViewModel>>(items);
        }

    }
}