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

        private static OurDeviceInformation instance;

        private OurDeviceInformation()
        { }

        public static OurDeviceInformation getInstance()
        {
            if (instance == null)
                instance = new OurDeviceInformation();
            return instance;
        }

        public DeviceInformation Device
        {
            get;
            set;
        }

    }
}
