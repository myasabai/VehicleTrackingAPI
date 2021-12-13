using System;
using System.Collections.Generic;

#nullable disable

namespace VehicleAPI.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserEmail { get; set; }
        public string UserRole { get; set; }
    }
}
