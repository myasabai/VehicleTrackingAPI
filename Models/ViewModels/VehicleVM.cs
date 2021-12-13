using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleAPI.Models.ViewModels
{
    public class VehicleVM
    {
        public int VehicleID { get; set; }
        public string VehicleName { get; set; }
        public string RegisterNo { get; set; }
        //public int? UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserEmail { get; set; }

    }
}
