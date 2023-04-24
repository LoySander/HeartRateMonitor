using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model.DatabaseModel
{
    [Table("companies")]
    public class Company
    {
        [Column("company_id")]
        public int Id { get; set; }
        [Column("title_company")]
        public string Name { get; set; }

        public List<Device> Devices { get; set; }
        
    }
}
