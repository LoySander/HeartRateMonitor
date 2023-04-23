using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model.DatabaseModel
{
    [Table("scenes")]
    public class Scene
    {
        [Column("scene_id")]
        public int Id { get; set; }
        [Column("title")]
        public string Name { get; set; }
        [Column("activity")]
        public int Activity { get; set; }
        [Column("scene_type")]
        public int Type { get; set; }
    }
}
