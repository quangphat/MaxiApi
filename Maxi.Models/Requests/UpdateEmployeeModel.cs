using System;
using System.Collections.Generic;
using System.Text;

namespace Maxi.Models.Requests
{
    public class UpdateEmployeeModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public int TeamId { get; set; }
    }
}
