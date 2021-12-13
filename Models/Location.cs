using System;
using System.Collections.Generic;

#nullable disable

namespace VehicleAPI.Models
{
    public partial class Location
    {
        public int Id { get; set; }
        public int? DeviceId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
