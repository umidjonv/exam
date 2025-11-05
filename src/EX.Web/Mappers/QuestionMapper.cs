using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EX.Data.Entities;
using EX.Web.Mappers.Profile;
using EX.Web.Models;
using X.PagedList;

namespace EX.Web.Mappers
{
    public static class QuestionMapper
    {

        private static readonly IMapper Mapper;

        static QuestionMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<QuestionProfile>())
                .CreateMapper();
        }

        public static IPagedList<QuestionViewModel> ToPager(this IPagedList<Question> items)
        {
            return Mapper.Map<IPagedList<QuestionViewModel>>(items);
        }

        public static Question ToEntity(this QuestionViewModel model)
        {
            return Mapper.Map<Question>(model, a => a.AfterMap((b, c) =>
            {
                c.Category = null;
            }));
        }

        public static QuestionViewModel ToModel(this Question model)
        {
            return Mapper.Map<QuestionViewModel>(model, a => a.AfterMap((b, c) =>
            {
                foreach (var correct in model.Corrects)
                {
                    var locale = c.Locales.FirstOrDefault(a => a.CultureId == correct.CultureId);

                    if (locale == null)
                        continue;

                    var answer = locale.Answers.FirstOrDefault(a => a.Id == correct.AnswerId);

                    if (answer == null)
                        continue;

                    answer.IsCorrect = true;
                }
            }));
        }

        public static IEnumerable<QuestionViewModel> ToCollection(this IEnumerable<Question> items)
        {
            return Mapper.Map<IEnumerable<QuestionViewModel>>(items);
        }

    }
}