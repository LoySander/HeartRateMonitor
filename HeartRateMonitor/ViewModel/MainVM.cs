using HeartRateMonitor.Model;
using HeartRateMonitor.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Enumeration;

namespace HeartRateMonitor.ViewModel
{
    class MainVM : INotifyPropertyChanged
    { 
        private RelayCommand openFindViewCommand;
        private RelayCommand startСommand;
        private RelayCommand disconnectCommand;
        private WindowService showService;
        private OurDeviceInformation device;
        private string _selectedDevice;
        private string _valueHeartRate;
        private MiBand authenticate;
        private ConnectionToBLE connection;
        private HeartRate heartRate;
        public event PropertyChangedEventHandler PropertyChanged;

        public MainVM()
        {
           showService = new WindowService();
           device = new OurDeviceInformation();
           authenticate = new MiBand();
           connection = ConnectionToBLE.getInstance();
           heartRate = new HeartRate(this);
            //ShowFindViewCommand = new RelayCommand(ShowFirstView);
        }

        public HeartRate HeartRate
        {
            get { return heartRate; }
            set { heartRate = value;
                OnPropertyChanged(nameof(HeartRate));
            }
        }
        public RelayCommand OpenFindViewCommand
        {
            get
            {
                return openFindViewCommand ??
                    (openFindViewCommand = new RelayCommand(obj =>
                    {
                        ShowFindView();
                    }));
            }
        }
        public RelayCommand DisconnectCommand
        {
            get
            {
                return disconnectCommand ??
                    (disconnectCommand = new RelayCommand(obj =>
                    {
                        connection.Disconnect(device.Device);
                        showService.ShowMessageBox(connection.GetBluetoothLE().ConnectionStatus.ToString());
                    }));
            }
        }

        public RelayCommand StartСommand
        {
            get
            {
                return startСommand ??
                    (startСommand = new RelayCommand(async obj =>
                    {
                        await heartRate.StartHeartrateMonitorAsync(connection.GetBluetoothLE());
                        SelectedDevice = device.Device.Name.ToString();
                    }));
            }
        }
        public void ShowFindView()
        {
            FindVM vm = new FindVM(this.device,this.showService);
            showService.ShowOrOpen(0, vm, true);     
        }
        public string SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                OnPropertyChanged(nameof(SelectedDevice));
            }
        }

        public string ValueHeartRate
        {
            get { return _valueHeartRate; }
            set
            {
                _valueHeartRate = value;
                OnPropertyChanged(nameof(ValueHeartRate));
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
