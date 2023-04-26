using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model.DatabaseModel.DTO
{
    public class SceneTypeDTO
    {
      
        public int Id { get; set; }
       
        public string Name { get; set; }

        public List<Scene> Scenes { get; set; }
    }
}
