using System.Collections.Generic;
using AutoMapper;
using EX.Common.Extensions;
using EX.Data.Entities;
using EX.Web.Mappers.Profile;
using EX.Web.Models;

namespace EX.Web.Mappers
{
    public static class ScheduleMapper
    {
        private static readonly IMapper _mapper;

        static ScheduleMapper()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<ScheduleProfile>())
                .CreateMapper();
        }

        public static IEnumerable<ScheduleViewModel> ToCollection(this IEnumerable<Schedule> items)
        {
            return _mapper.Map<IEnumerable<ScheduleViewModel>>(items);
        }

        public static Schedule ToEntity(this ScheduleViewModel model)
        {
            return _mapper.Map<Schedule>(model, a => a.AfterMap((b, c) =>
            {
                c.Exam = null;
                c.StartDate = model.StartDate.ConvertLongDate().GetValueOrDefault();
            }));
        }

        public static ScheduleViewModel ToModel(this Schedule model)
        {
            return _mapper.Map<ScheduleViewModel>(model);
        }

    }
}