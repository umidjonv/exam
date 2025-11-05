using AutoMapper;
using EX.Web.Mappers.Profile;
using System.Collections.Generic;
using EX.Data.Entities;
using EX.Web.Models;

namespace EX.Web.Mappers
{

    public static class CategoryMapper
    {
        private static readonly IMapper Mapper;

        static CategoryMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<CategoryProfile>())
                .CreateMapper();
        }

        public static IEnumerable<CategoryViewModel> ToCollection(this IEnumerable<Category> items)
        {
            return Mapper.Map<IEnumerable<CategoryViewModel>>(items);
        }

        public static Category ToEntity(this CategoryViewModel model)
        {
            return Mapper.Map<Category>(model);
        }

        public static CategoryViewModel ToModel(this Category model)
        {
            return Mapper.Map<CategoryViewModel>(model);
        }

    }
}
