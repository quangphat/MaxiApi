using System;
using System.Collections.Generic;
using System.Text;

namespace Maxi.Repository.Entities
{
    public class USPEmployee: BaseUspEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public string TeamName { get; set; }
        public int  TeamId { get; set; }
        public int LeaderId { get; set; }
        public string LeaderName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
