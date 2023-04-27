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
    public class SceneRepository: IRepository<Scene>
    {
        private ApplicationContext _context;

        public SceneRepository()
        {
            _context = new ApplicationContext();
        }
        public void Create(Scene item)
        {
            _context.Scenes.Add(item);
        }

        public void Delete(int id)
        {
            Scene scene = _context.Scenes.Find(id);
            if (scene != null)
                _context.Scenes.Remove(scene);
        }

        public void Dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Scene> GetAllList()
        {

            return _context.Scenes.Include(c=>c.SceneType).ToList();
        }

        public Scene GetObject(int id)
        {
            return _context.Scenes.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Scene item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
