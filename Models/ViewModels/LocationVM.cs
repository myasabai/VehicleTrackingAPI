using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleAPI.Models.ViewModels
{
    public class LocationVM
    {
        public int? DeviceID  { get; set; }
        public int? UserID { get; set; } // Updated User
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime UpdateTime { get; set; } //Every 30 secs
        

    }
}
