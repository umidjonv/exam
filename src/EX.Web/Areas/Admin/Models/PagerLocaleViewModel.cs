using System.Collections.Generic;
using EX.Common.Core;
using EX.Web.Dtos;
using X.PagedList;

namespace EX.Web.Areas.Admin.Models
{
    public class PagerLocaleViewModel<T> where T : BaseEntity
    {

        public IPagedList<T> Items { get; set; }

        public IEnumerable<CultureDto> Cultures { get; set; }

        public string Lang { get; set; }

    }
}