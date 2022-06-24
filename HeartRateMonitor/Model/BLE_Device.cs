using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace HeartRateMonitor.Model
{
    class BLE_Device : INotifyPropertyChanged
    {
        static private DeviceInformation device = null;
        private DeviceWatcher deviceWatcher = null;
        public List<DeviceInformation> devices = null;
        private List<string> names = null;
        //public readonly ReadOnlyObservableCollection<DeviceInformation> myPublicDevices;

        public BLE_Device()
        {
            devices = new List<DeviceInformation>();
            names = new List<string>();       
        }

        public List<DeviceInformation> Devices
        {
            get { return devices; }

            set {
                devices = value;
                OnPropertyChanged("Device");
            }

        }
        public List<string> Names
        {
            get { return names; }

            set
            {
                names = value;
                OnPropertyChanged("Name");
            }

        }
        public DeviceInformation Device
        {
            get { return device; }
            set
            {
                device = value;
                OnPropertyChanged("Device");
            }
        }
        public void AddDevice(DeviceInformation device)
        {
            devices.Add(device);
            names.Add(device.Name.ToString());
            // OnPropertyChanged("FindDevice");
        }
        public void FindBLE_Device()
        {
            string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };

            deviceWatcher =
                        DeviceInformation.CreateWatcher(
                                BluetoothLEDevice.GetDeviceSelectorFromPairingState(false),
                                requestedProperties,
                                DeviceInformationKind.AssociationEndpoint);


            // Register event handlers before starting the watcher.
            // Added, Updated and Removed are required to get all nearby devices
            deviceWatcher.Added += DeviceWatcher_Added;
            deviceWatcher.Updated += DeviceWatcher_Updated;
            deviceWatcher.Removed += DeviceWatcher_Removed;

            // EnumerationCompleted and Stopped are optional to implement.
            deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
            deviceWatcher.Stopped += DeviceWatcher_Stopped;

            // Start the watcher.
            deviceWatcher.Start();
            while (true)
            {
                if (device == null)
                {
                    Thread.Sleep(200);
                }
                else if (names.Contains(device.Name.ToString()) == false || device.Name.ToString() == "Mi Band 3")
                {                  
                    AddDevice(device);
                    if (device.Name.ToString() == "Mi Band 3")
                    {
                        break;
                    }
                }
            }

            deviceWatcher.Stop();
        }
        private static void DeviceWatcher_Stopped(DeviceWatcher sender, object args)
        {
            //throw new NotImplementedException();
        }

        private static void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            //throw new NotImplementedException();
        }
        private static void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            //throw new NotImplementedException();
        }

        private static void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            //throw new NotImplementedException();
        }
        private static void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
           
            if (args.Name.ToString() != "")
            {
              // Thread.Sleep(100);
                device = args;
                Thread.Sleep(500);
            }
        }

    public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
