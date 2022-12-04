using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model
{
    public class ModelInput
    {
        [LoadColumn(0)]
        public float Rate { get; set; }
    }
}
