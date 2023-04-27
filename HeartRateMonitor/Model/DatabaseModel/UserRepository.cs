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
    public class UserRepository: IRepository<User>  
    {
        private ApplicationContext _context;

        public UserRepository()
        {
            _context = new ApplicationContext();
        }
        public void Create(User item)
        {
            _context.Users.Add(item);
        }

        public void Delete(int id)
        {
           User user = _context.Users.Find(id);
            if (user != null)
                _context.Users.Remove(user);
        }

        public void Dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

        public IEnumerable<User> GetAllList()
        {

            return _context.Users.ToList();
        }

        public User GetObject(int id)
        {
            return _context.Users.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(User item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
