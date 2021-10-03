using System;
using System.Collections.Generic;

#nullable disable

namespace Maxi.Repository
{
    public partial class Team:EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentTeamId { get; set; }
        public int LeaderId { get; set; }
       
    }
}
