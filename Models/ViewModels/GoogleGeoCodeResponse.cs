using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleAPI.Models.ViewModels
{
    public class GoogleGeoCodeResponse
    {
        public string status { get; set; }

        public results[] results { get; set; }
    }
}
