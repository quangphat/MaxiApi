using System;
using System.Collections.Generic;
using System.Text;

namespace Maxi.Repository
{
    public abstract class EntityBase
    {
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
