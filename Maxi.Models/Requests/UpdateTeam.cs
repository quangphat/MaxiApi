using System;
using System.Collections.Generic;
using System.Text;

namespace Maxi.Models.Requests
{
    public class UpdateTeam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int LeaderId { get; set; } 
    }
}
