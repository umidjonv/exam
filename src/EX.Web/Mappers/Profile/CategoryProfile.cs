using AutoMapper;
using EX.Data.Entities;
using EX.Web.Models;

namespace EX.Web.Mappers.Profile
{
    public class CategoryProfile : AutoMapper.Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryViewModel>(MemberList.Destination)
                .ReverseMap(); 
        }
    }
}
