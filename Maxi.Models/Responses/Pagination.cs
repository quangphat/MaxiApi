using System;
using System.Collections.Generic;
using System.Text;

namespace Maxi.Models.Responses
{
    public class Pagination<T> where T : class
    {
        public List<T> Datas { get; set; }
        public long TotalRecord { get; set; }
    }
}
