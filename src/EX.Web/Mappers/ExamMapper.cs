using AutoMapper;
using EX.Data.Entities;
using EX.Web.Mappers.Profile;
using EX.Web.Models;
using X.PagedList;

namespace EX.Web.Mappers
{
    public static class ExamMapper
    {

        private static readonly IMapper Mapper;

        static ExamMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ExamProfile>())
                .CreateMapper();
        }

        public static IPagedList<ExamViewModel> ToPager(this IPagedList<Exam> items)
        {
            return Mapper.Map<IPagedList<ExamViewModel>>(items);
        }

        public static Exam ToEntity(this ExamViewModel model)
        {
            return Mapper.Map<Exam>(model, a => a.AfterMap((b, c) =>
            {
                c.Options.ForEach(d =>
                {
                    d.Category = null;
                });
            }));
        }

        public static ExamViewModel ToModel(this Exam model)
        {
            return Mapper.Map<ExamViewModel>(model);
        }

    }
}