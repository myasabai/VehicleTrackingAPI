using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehicleAPI.Interfaces;
using VehicleAPI.Managers;
using VehicleAPI.Models;

namespace VehicleAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class VehicleDIApiController : ControllerBase
    {
       // private VehicleTrackingContext _context;
        private VehicleDIManager _vhiDiMgr;
        //private IRepository<Vehicle> _vhcRepo;
        //private IRepository<Location> _locRepo;
        //private IRepository<User> _userRepo;
        //private IRepository<Device> _devRepo;
        public VehicleDIApiController(IRepository<Vehicle> vhcRepo, IRepository<Location> locRepo, IRepository<User> userRepo, IRepository<Device> devRepo)
        {
           // _context = context;
            //_vhcRepo = new BaseRepository<Vehicle>(_context);
            //_locRepo = new BaseRepository<Location>(_context);
            //_userRepo = new BaseRepository<User>(_context);
            //_devRepo = new BaseRepository<Device>(_context);
            _vhiDiMgr = new VehicleDIManager(vhcRepo, locRepo, userRepo, devRepo);
        }

        [Route("api/vehicle/getcurrentpositiondi")]
        public async Task<IActionResult> GetCurrentPosition(int userID, int deviceID)
        {
            Location result = await _vhiDiMgr.GetCurrentPosition(userID, deviceID);
            if (result != null)
                return Ok(result);
            else
                return BadRequest();
        }
    }
}
