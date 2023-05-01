using HeartRateMonitor.Interfaces;
using HeartRateMonitor.Model.DatabaseModel.Context;
using HeartRateMonitor.Model.DatabaseModel.DTO;
using HeartRateMonitor.ViewModel.DBViewModel;
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
                                  Id = d.Id,
                                  Name = d.Name,
                                  Type = d.Type,
                                  Company = d.Company
                              };

                return devices.ToList();
         
        }

        public IEnumerable<DeviceVM> GetAllDeviceDTO()
        {
            List<DeviceVM> deviceVMs = new List<DeviceVM>();
            foreach(var x in _deviceRepository.GetAllList())
            {
             
                deviceVMs.Add(new DeviceVM(new DeviceDTO
                {
                    Name = x.Name,
                    Type = x.Type,
                    Company = x.Company
                }));
            }


            return deviceVMs;

        }

        public void AddNewObject(DeviceDTO deviceDTO)
        {
            Device device = new Device()
            {
                Company = deviceDTO.Company,
                Name = deviceDTO.Name,
                Type = 0,
                IdCompany = deviceDTO.Company.Id
               
            };
            _deviceRepository.Create(device);
        }

        public void UpdateObject(DeviceDTO deviceDTO)
        {
            Device device = _deviceRepository.GetObject(deviceDTO.Id);
            device.Name = deviceDTO.Name.ToString();
            device.Company = deviceDTO.Company;
            device.Type = deviceDTO.Type;
            _deviceRepository.Update(device);
        }

        public void  DeleteObject(DeviceDTO deviceDTO)
        {
            _deviceRepository.Delete(deviceDTO.Id); 
        }

        public IEnumerable<Company> GetAllCompany()
        {
            using (ApplicationContext context = new ApplicationContext())
            {
               // var x = context.Devices.ToList();
               var z = context.Companies.ToList();
                //var companies = from u in context.Companies
                //            //select new CompanyDTO
                //            //{  
                //            //    Name = u.Name,
                //            //};
                return z.ToList();
            }
        }
    }
}
