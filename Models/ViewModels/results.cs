using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleAPI.Models.ViewModels
{
    public class results
    {
        public string formatted_address { get; set; }

        public geometry geometry { get; set; }

        public string[] types { get; set; }

        public address_component[] address_components { get; set; }
    }
}
