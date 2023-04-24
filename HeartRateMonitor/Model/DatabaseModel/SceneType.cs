using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model.DatabaseModel
{
    [Table("scene_types")]
    public class SceneType
    {

        [Column("type_id")]
        public int Id { get; set; }
        [Column("type")]
        public string Name { get; set; }

        public List<Scene> Scenes { get; set; }
        
    }
}
