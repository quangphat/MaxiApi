using System;
using System.Collections.Generic;
using System.Text;

namespace Maxi.Repository.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
