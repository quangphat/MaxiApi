using System;
using System.Collections.Generic;

#nullable disable

namespace Maxi.Repository
{
    public partial class Employee
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public DateTime? Birthday { get; set; }
        public int LevelId { get; set; }
        public string Phone { get; set; }
        public int TeamId { get; set; }
        public int LeaderId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
