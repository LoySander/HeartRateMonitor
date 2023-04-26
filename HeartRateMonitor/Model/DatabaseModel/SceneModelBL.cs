using HeartRateMonitor.Model.DatabaseModel.Context;
using HeartRateMonitor.Model.DatabaseModel.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model.DatabaseModel
{
    public class SceneModelBL
    {
        public IEnumerable<SceneDTO> GetAllScenes()
        {
            using (ApplicationContext context = new ApplicationContext())
            {
                var scenes = from d in context.Scenes
                              select new SceneDTO
                              {
                                  Id = d.Id,
                                  Name = d.Name,
                                  Activity = d.Activity,
                                  Type = d.Type,
                                  SceneType = d.SceneType,
            
                              };
                return scenes.ToList();
            }
        }

        public IEnumerable<SceneDTO> GetAllScenesWithType()
        {
            using (ApplicationContext context = new ApplicationContext())
            {
                var scenes = from u in context.Scenes
                            join c in context.SceneTypes on u.Type equals c.Id
                            select new SceneDTO
                            {
                                Id = u.Id,
                                Name = u.Name,
                                Activity = u.Activity,
                                Type = u.Type,
                                SceneType = c
                            };
                return scenes.ToList();
            }
        }
    }
}
