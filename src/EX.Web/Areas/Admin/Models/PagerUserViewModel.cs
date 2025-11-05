using System.Collections.Generic;
using EX.Common.Core;
using EX.Web.Dtos;

namespace EX.Web.Areas.Admin.Models
{
    public class PagerUserViewModel<T> : PagerLocaleViewModel<T> where T : BaseEntity
    {

        public IDictionary<string, UserEntityDto> Users { get; set; }

        public UserEntityDto Current { get; set; }

    }
}