using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleAPI.Interfaces;
using VehicleAPI.Models;

namespace VehicleAPI.Managers
{
    public class VehicleDIManager
    {
       // private VehicleTrackingContext _context;
        private IRepository<Vehicle> _vehRepo;
        private IRepository<Location> _locRepo;
        private IRepository<User> _userRepo;
        private IRepository<Device> _devRepo;


        public VehicleDIManager(IRepository<Vehicle> vhcRepo, IRepository<Location> locRepo, IRepository<User> userRepo, IRepository<Device> devRepo)
        {
           // _context = context;
            _vehRepo = vhcRepo;
            _locRepo = locRepo;
            _userRepo = userRepo;
            _devRepo = devRepo;

        }

        public async Task<Location> GetCurrentPosition(int userID, int deviceID)
        {

            try
            {
                //int vhcID = await this.GetDevIDbyUser(userID);
                bool vhc = await this.UserbyDevice(userID, deviceID);
                if (vhc)
                {
                    Location loc = await _locRepo.GetAll().Where(a => a.DeviceId == deviceID).OrderBy(a => a.UpdateTime).LastOrDefaultAsync();
                    return loc;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UserbyDevice(int userID, int deviceID)
        {
            bool vhcUser = await (from vhc in _vehRepo.GetAll()
                                  join dev in _devRepo.GetAll()
                                  on vhc.Id equals dev.VehicleId
                                  where userID == vhc.UserId && deviceID == dev.Id
                                  select vhc).AnyAsync();

            return vhcUser;
        }


    }
}
