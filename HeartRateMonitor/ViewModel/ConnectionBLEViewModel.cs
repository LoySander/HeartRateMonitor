using HeartRateMonitor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;

namespace HeartRateMonitor.ViewModel
{
    class ConnectionBLEViewModel:INotifyPropertyChanged
    {
        private BLE_Device deviceBLE;
        private ObservableCollection<DeviceInformation> _devices;
        private DeviceInformation _device;
        public event PropertyChangedEventHandler PropertyChanged;

        public ConnectionBLEViewModel()
        {
            deviceBLE = BLE_Device.getInstance();
            Devices = new ObservableCollection<DeviceInformation>();
        }

        #region command
        private RelayCommand _findCommand;

        public RelayCommand FindCommand
        {
            get
            {
                return _findCommand ??
                    (_findCommand = new RelayCommand(obj =>
                    {
                        deviceBLE.FindBLE_Device();
                        Devices = CollectionEx.ToObservableCollection(deviceBLE.Devices);
                        //GenerateBLE_Devices();
                    }));
            }
        }
        #endregion

        public ObservableCollection<DeviceInformation> Devices
        {
            get=> _devices;
            set
            {
                _devices = value;
                OnPropertyChanged();
            }
        }

        public DeviceInformation Device
        {
            get => _device;
            set 
            {
                _device = value;
                OnPropertyChanged(); 
            }

        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
