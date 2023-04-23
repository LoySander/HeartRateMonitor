using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model.DatabaseModel
{
    [Table("physiological_parameters")]
    public class Physiological_parameter
    {
        [Column("physiological_parameter_id")]
        public int Id { get; set; }
        [Column("heart_rate")]
        public string HeartRate { get; set; }
        [Column("pressure")]
        public string Pressure { get; set; }
        [Column("temperature")]
        public string Temperature { get; set; }
        [Column("oxygen_saturation")]
        public string Saturation { get; set; }
    }
}
