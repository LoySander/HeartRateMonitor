using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;

namespace HeartRateMonitor.Services
{
    class OurDeviceInformation
    {
        static private DeviceInformation device = null;

        public DeviceInformation Device
        {
            get;
            set;
        }

    }
}
