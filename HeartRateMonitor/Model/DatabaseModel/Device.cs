using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model.DatabaseModel
{
    public class Device
    {
        [Column("device_id")]
        public int Id { get; set; }
        [Column("company_id")]
        public int IdCompany { get; set; }
        [Column("title_device")]
        public string Name { get; set; }
        [Column("type")]
        public Byte? Type { get; set; }
       
    }
}
