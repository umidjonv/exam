using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EX.Common;
using EX.Common.Rest;
using EX.Web.Dtos;

namespace EX.Web.Services
{
    public class HandbookService : ApiClient
    {
        public HandbookService(AppConfig config) : base(config.HandbookApi)
        {
        }

        public async Task<IEnumerable<RegionDto>> GetRegions()
        {
            try
            {
                return await Get<IEnumerable<RegionDto>>("/Region/GetAll/1");
            }
            catch(Exception e)
            {
                return new RegionDto[0];
            }
        }

        public async Task<IEnumerable<CultureDto>> GetCultures()
        {
            try
            {
                var list = new List<CultureDto>
                { 
                    new CultureDto { Id = "en-US", Name = "English (United States)", Code = "en-US" },
                    new CultureDto { Id = "ru-RU", Name = "Русский", Code = "ru-RU" },
                    new CultureDto { Id = "uz-UZ", Name = "O'zbekcha", Code = "uz-UZ" },
                    
                };
                    /*await Get<IEnumerable<CultureDto>>("/Culture/GetAll");*/

                return list.OrderByDescending(a => a.Id);
            }
            catch
            {
                return new CultureDto[0];
            }
        }

    }
}
