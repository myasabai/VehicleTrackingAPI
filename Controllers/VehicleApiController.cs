using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehicleAPI.Interfaces;
using VehicleAPI.Managers;
using VehicleAPI.Models;
using VehicleAPI.Models.ViewModels;

namespace VehicleAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class VehicleApiController : ControllerBase
    {

        private VehicleManager _vhcMgr;
        public VehicleApiController()
        {
            _vhcMgr = new VehicleManager();
        }

        [HttpPost]
        [Route("api/vehicle/addvehicle")]
        public async Task<IActionResult> InsertVehicle(VehicleVM vhc)
        {
            Vehicle result = await _vhcMgr.InsertVehicle(vhc);
            if (result != null)
                return Ok(result);
            else
                return BadRequest();
        }

        [HttpPost]
        [Route("api/vehicle/updatelocation")]
        public async Task<IActionResult> UpdateLocation(LocationVM locdata)
        {
            Location result = await _vhcMgr.UpdateLocation(locdata);
            if (result != null)
                return Ok(result);
            else
                return BadRequest();
        }

        [Authorize]
        [Route("api/vehicle/getcurrentposition")]
         public async Task<IActionResult> GetCurrentPosition(int userID, int deviceID)
        {
            Location result = await _vhcMgr.GetCurrentPosition(userID, deviceID);
            if (result != null)
                return Ok(result);
            else
                return BadRequest();
        }

        [Authorize]
        [Route("api/vehicle/getpositionsbytime")]
        public async Task<IActionResult> GetPositionbyTime(int userID, int deviceID, DateTime starttime, DateTime endtime)
        {
            List<Location> result = await _vhcMgr.GetPositions(userID, deviceID, starttime,endtime);
            if (result != null)
                return Ok(result);
            else
                return BadRequest();
        }

        [Route("api/vehicle/getlocality")]
        public async Task<IActionResult> GetLocality(string latitude, string longitude)
        {
            List<string> result = await  _vhcMgr.GetLocalityByGeoLocation(longitude, latitude);
            if (result != null)
                return Ok(result);
            else
                return BadRequest();
        }






    }


}
