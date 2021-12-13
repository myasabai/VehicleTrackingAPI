using System;
using System.Collections.Generic;

#nullable disable

namespace VehicleAPI.Models
{
    public partial class Device
    {
        public int Id { get; set; }
        public int? VehicleId { get; set; }
    }
}
