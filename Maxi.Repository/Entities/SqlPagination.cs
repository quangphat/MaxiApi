using System;
using System.Collections.Generic;
using System.Text;

namespace Maxi.Repository.Entities
{
    public abstract class SqlPagination
    {
        public long TotalRecord { get; set; }
    }
}
