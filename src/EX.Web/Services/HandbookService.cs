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
                var list = await Get<IEnumerable<CultureDto>>("/Culture/GetAll");

                return list.OrderByDescending(a => a.Id);
            }
            catch
            {
                return new CultureDto[0];
            }
        }

    }
}
