using HeartRateMonitor.Interfaces;
using HeartRateMonitor.Model.DatabaseModel.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model.DatabaseModel
{
    public class DeviceRepository : IRepository<Device>
    {
        private ApplicationContext _context;

        public DeviceRepository()
        {
            _context = new ApplicationContext();
        }
        public void Create(Device item)
        {
           _context.Devices.Add(item);
           _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Device device = _context.Devices.Find(id);
            if (device != null)
                _context.Devices.Remove(device);
        }

        public void Dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Device> GetAllList()
        {

            return _context.Devices.Include(c => c.Company).ToList();
        }

        public Device GetObject(int id)
        {
            return _context.Devices.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Device item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
