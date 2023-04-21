using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model.DatabaseModel
{
    public class User
    {
        [Column("user_id")]
        public int Id { get; set; }
        [Column("surname")]
        public string Surname { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("middle_name")]
        public string MiddleName { get; set; }
        [Column("birthday")]
        public DateTime Birthday { get; set; }
        [Column("average_heart_rate")]
        public int AverageHeartRate { get; set; }
    }
}
