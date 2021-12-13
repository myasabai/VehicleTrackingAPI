using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using VehicleAPI.Interfaces;
using VehicleAPI.Models;
using VehicleAPI.Models.ViewModels;

namespace VehicleAPI.Managers
{
    public class VehicleManager
    {
        IRepository<Vehicle> _vehRepo;
        IRepository<Location> _locRepo;
        IRepository<User> _userRepo;
        IRepository<Device> _devRepo;
        VehicleTrackingContext _ctx;

        public VehicleManager()
        {
            _ctx = new VehicleTrackingContext();
            _vehRepo = new BaseRepository<Vehicle>(_ctx);
            _locRepo = new BaseRepository<Location>(_ctx);
            _userRepo = new BaseRepository<User>(_ctx);
            _devRepo = new BaseRepository<Device>(_ctx);
        }

        public async Task<Vehicle> InsertVehicle(VehicleVM vhc)
        {
            Vehicle updatedEntity = new Vehicle();

            try
            {
                string olduser = await _userRepo.GetAll().Where(a => a.UserEmail == vhc.UserEmail || (a.UserName == vhc.UserName && a.Password == vhc.Password)).Select(a => a.UserEmail).FirstOrDefaultAsync();

                if (string.IsNullOrWhiteSpace(olduser))
                {
                    if (vhc.VehicleID == 0)
                    {
                        User newuser = new User()
                        {
                            UserName = vhc.UserName,
                            Password = vhc.Password,
                            UserEmail = vhc.UserEmail,
                            UserRole = "Administrator"
                        };

                        User updatedUser = await _userRepo.Insert(newuser);

                        Vehicle newvhc = new Vehicle()
                        {
                            VehicleName = vhc.VehicleName,
                            RegisterNo = vhc.RegisterNo,
                            Accesstime = DateTime.UtcNow,
                            UserId = newuser.Id,
                        };
                        updatedEntity = await _vehRepo.Insert(newvhc);

                        Device dev = new Device()
                        {
                            VehicleId = newvhc.Id
                        };

                        Device updatedDevice = await _devRepo.Insert(dev);

                    }
                    return updatedEntity;
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

        public async Task<Location> UpdateLocation(LocationVM loc)
        {
            Location updatedLoc = new Location();
            try
            {
                //bool vhcUser = await _vehRepo.GetAll().Where(a => a.Id == loc.VehicleID && a.UserId == loc.UserID).AnyAsync();
                bool vhcUser = await this.UserbyDevice(loc.UserID.Value, loc.DeviceID.Value);

                if (vhcUser) // to ensure a user update his or her own device.
                {
                    if (loc.DeviceID != 0 || loc.DeviceID != null)
                    {
                        Location locdata = new Location
                        {
                            DeviceId = loc.DeviceID,
                            Latitude = loc.Latitude,
                            Longitude = loc.Longitude,
                            UpdateTime = loc.UpdateTime
                        };
                        updatedLoc = await _locRepo.Insert(locdata);
                        return updatedLoc;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public async Task<List<Location>> GetPositions(int userID, int deviceID, DateTime starttime, DateTime endtime)
        {
            try
            {
                //int vhcID = await this.GetDevIDbyUser(userID);
                bool vhc = await this.UserbyDevice(userID, deviceID);
                if (vhc)
                {
                    List<Location> loc = await _locRepo.GetAll().Where(a => a.DeviceId == deviceID && a.UpdateTime.Value >= starttime && a.UpdateTime.Value <= endtime).ToListAsync();
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

        public async Task<int> GetDevIDbyUser(int userID)
        {
            int vhcID = await _vehRepo.GetAll().Where(a => a.UserId == userID).Select(a => a.Id).FirstOrDefaultAsync();
            return vhcID;
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

        public async Task<List<string>> GetLocalityByGeoLocation(string latitude, string longitude)
        {
            var apikey = ""; // API Key from geocode api

            var baseUri = "https://maps.googleapis.com/maps/api/geocode/json" + "?latlng={0},{1}&key=apikey";

            string url = string.Format(baseUri, latitude, longitude);

            //Make the API call
            var json = new WebClient().DownloadString(url);
            //Deserialize the result to GoogleGeoCodeRespond
            var jsonResult = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(json);
            List<string> res = new List<string>();
            //check if status is ok
            if (jsonResult.status == "OK")
            {
                //loop through the result for addresses
                for (int i = 0; i < jsonResult.results.Length; i++)
                {
                    //ouput the result addresses
                    res.Add(jsonResult.results[i].formatted_address);
                }
            }
            else
            {
                //Error status
                res.Add(jsonResult.status);
            }
            return res;
        }
    }
}
