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

        public  IEnumerable<Device> GetAllDevices()
        {
            //using(ApplicationContext context = new ApplicationContext())
            //{
            //    /// Надо ОБЯЗАТЕЛЬНО избавиться от DataSet
            //    /// Выше по уровне он не должен подымать ни в коем случае.
            //    //var devices = from d in context.Devices
            //    //              select new DeviceDTO
            //    //              {
            //    //                  Id = d.Id,
            //    //                  IdCompany = d.IdCompany,
            //    //                  Name = d.Name,
            //    //                  Type = d.Type,
            //    //                  Company = d.Company
            //    //              };
            //    var x = context.Companies.ToList();
              
            //   // return context.Devices.ToList();
            //}

            return _deviceRepository.GetAllList();
        }

        public IEnumerable<DeviceDTO> GetAllDevicesWithCompany()
        {
            using (ApplicationContext context = new ApplicationContext())
            {
                var x = context.Devices.ToList();
                var z = context.Companies.ToList();
                var users = from u in context.Devices
                            join c in context.Companies on u.IdCompany equals c.Id
                            select new DeviceDTO {
                                Id = u.Id,
                                IdCompany = u.IdCompany,
                                Name = u.Name,
                                Type = u.Type,
                                Company = c
                            };
                return users.ToList();
            }
        }
    }
}
