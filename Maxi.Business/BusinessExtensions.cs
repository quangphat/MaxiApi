using Maxi.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maxi.Business
{
    public static class BusinessExtensions
    {
        public static void ProcessPaging(ref int page, ref int limit, int maxOflimit = 1000)
        {
            page = page <= 0 ? 1 : page;
            limit = (limit <= 0 || limit >= maxOflimit) ? maxOflimit : limit;
        }

        public static Pagination<Tout> ToPaging<Tout>(long total, List<Tout> datas) where Tout : class
        {
            if (datas == null)
                return null;

            return new Pagination<Tout>()
            {
                TotalRecord = total,
                Datas = datas
            };
        }
    }
}
