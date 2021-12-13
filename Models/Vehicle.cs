using System;
using System.Collections.Generic;

#nullable disable

namespace VehicleAPI.Models
{
    public partial class Vehicle
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string VehicleName { get; set; }
        public string RegisterNo { get; set; }
        public DateTime? Accesstime { get; set; }
    }
}
