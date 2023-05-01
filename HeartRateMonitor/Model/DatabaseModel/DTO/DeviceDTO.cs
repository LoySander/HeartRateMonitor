using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model.DatabaseModel.DTO
{
    public class DeviceDTO
    {
       
        public int Id { get; set; }
        
       // public int IdCompany { get; set; }
        
        public string Name { get; set; }
       
        public Byte? Type { get; set; }

        public Company Company { get; set; }
    }
}
