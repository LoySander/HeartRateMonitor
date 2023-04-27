using HeartRateMonitor.Interfaces;
using HeartRateMonitor.Model.DatabaseModel.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model.DatabaseModel
{
    public class Physiological_parameterRepository: IRepository<Physiological_parameter>
    {
        private ApplicationContext _context;

        public Physiological_parameterRepository()
        {
            _context = new ApplicationContext();
        }
        public void Create(Physiological_parameter item)
        {
            _context.Parameters.Add(item);
        }

        public void Delete(int id)
        {
            Physiological_parameter param = _context.Parameters.Find(id);
            if (param != null)
                _context.Parameters.Remove(param);
        }

        public void Dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Physiological_parameter> GetAllList()
        {

            return _context.Parameters.ToList();
        }

        public Physiological_parameter GetObject(int id)
        {
            return _context.Parameters.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Physiological_parameter item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
