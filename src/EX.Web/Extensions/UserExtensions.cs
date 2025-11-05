using System.Collections.Generic;
using System.Linq;
using EX.Web.Dtos;

namespace EX.Web.Extensions
{
    public static class UserExtension
    {
        public static string GetAttributeValue(this UserEntityDto userEntity, string key)
        {

            if (userEntity.Attributes == null || !userEntity.Attributes.Any())
                return string.Empty;

            return userEntity.Attributes.GetValueOrDefault(key)?.FirstOrDefault();
        }

    }
}
