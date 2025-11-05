using AutoMapper;
using EX.Data.Entities;
using EX.Web.Models;

namespace EX.Web.Mappers.Profile
{
    public class UserInExamProfile :AutoMapper.Profile
    {
        public UserInExamProfile()
        {
            CreateMap<UserInExamViewModel, UserInExams>(MemberList.Destination)
                .ReverseMap();

        }
    }
}
