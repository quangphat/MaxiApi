using System;
using System.Collections.Generic;
using System.Text;

namespace Maxi.Repository.Entities
{
    public class USPTeam:BaseUspEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LeaderName { get; set; }
        public int LeaderId { get; set; }
        public string ParentName { get; set; }
        public int ParentTeamId { get; set; }
    }
}
