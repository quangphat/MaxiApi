using System;
using System.Collections.Generic;

#nullable disable

namespace Maxi.Repository
{
    public partial class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentTeamId { get; set; }
        public int LeaderId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
