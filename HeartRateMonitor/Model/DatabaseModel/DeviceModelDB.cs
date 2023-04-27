using HeartRateMonitor.Interfaces;
using HeartRateMonitor.Model.DatabaseModel.Context;
using HeartRateMonitor.Model.DatabaseModel.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model.DatabaseModel
{
    public class DeviceModelDB
    {
        private IRepository<Device> _deviceRepository;

        public DeviceModelDB()
        {
            _deviceRepository = new DeviceRepository();
        }

        public  IEnumerable<DeviceDTO> GetAllDevices()
        {
           
                var devices = from d in _deviceRepository.GetAllList()
                              select new DeviceDTO
                              {

                                  Name = d.Name,
                                  Type = d.Type,
                                  Company = d.Company
                              };

                return devices.ToList();
         
        }

        //public IEnumerable<DeviceDTO> GetAllDevicesWithCompany()
        //{
        //    //using (ApplicationContext context = new ApplicationContext())
        //    //{
        //    //    var x = context.Devices.ToList();
        //    //    var z = context.Companies.ToList();
        //    //    var users = from u in context.Devices
        //    //                join c in context.Companies on u.IdCompany equals c.Id
        //    //                select new DeviceDTO {
        //    //                    Id = u.Id,
        //    //                    IdCompany = u.IdCompany,
        //    //                    Name = u.Name,
        //    //                    Type = u.Type,
        //    //                    Company = c
        //    //                };
        //    //    return users.ToList();
        //    //}
        //}
    }
}
