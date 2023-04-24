using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model.DatabaseModel.DTO
{
    public class SceneDTO
    {
       
        public int Id { get; set; }
       
        public string Name { get; set; }
       
        public int Activity { get; set; }
      
        public int Type { get; set; }

        public SceneType SceneType { get; set; }
    }
}
