using HeartRateMonitor.Model;
using HeartRateMonitor.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;

namespace HeartRateMonitor.ViewModel
{
    class FindVM : INotifyPropertyChanged
    {
        private BLE_Device device = new BLE_Device();
        private ObservableCollection<DeviceInformation> Devices => GetDevices();
        public ObservableCollection<string> names;
        public event PropertyChangedEventHandler PropertyChanged;
        private string _selectedDevice;
        private WindowService showService;
        private MessageBoxService messageBoxService;
        private OurDeviceInformation deviceInfromation;
        private ConnectionToBLE connection;
        private MessageVM messageVM;


        public FindVM(OurDeviceInformation device,WindowService service)
        {
            names = new ObservableCollection<string>();
            deviceInfromation = device;
            showService =service;
            connection = ConnectionToBLE.getInstance();
            messageBoxService = new MessageBoxService();
            messageVM = new MessageVM(messageBoxService,showService);
            GenerateBLE_Devices();
        }
        
        private RelayCommand findCommand;
        private RelayCommand cancelCommand;
        private RelayCommand connectCommand;
        public RelayCommand FindCommand
        {
            get
            {
                return findCommand ??
                    (findCommand = new RelayCommand(obj =>
                    {
                        //device.FindBLE_Device(); 

                        GenerateBLE_Devices();
                    }));              
            }
        }
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                    (cancelCommand = new RelayCommand(obj =>
                    {
                        //device.FindBLE_Device(); 
                        SelectedDevice = " ";
                    }));
            }
        }


        private void GenerateBLE_Devices()
        {
            //device.Names.Clear();
            device.Devices.Clear();
            device.Device = null;
            Names.Clear();
            device.FindBLE_Device();
           // GetNames();
            GetDevices();
        }

        public string SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                OnPropertyChanged(nameof(SelectedDevice));
                if (_selectedDevice != null)
                {
                    showService.ShowMessageBox("Вы выбрали " +_selectedDevice);
                }
            }
        }

        //public void GetNames()
        //{
        //    //var templist = new ObservableCollection<string>();
        //    foreach (var item in device.Names)
        //    {
        //        names.Add(item.ToString());
        //    }
        //   // return templist;
        //}

        public ObservableCollection<string> Names
        {
            get { return names; }
            set
            {
                names = value;
                OnPropertyChanged("Name");
            }
        }

        public ObservableCollection<DeviceInformation> GetDevices()
        {
            var templist = new ObservableCollection<DeviceInformation>();
            foreach (var item in device.Devices)
            {
                templist.Add(item);
            }
            return templist;
        }

        public RelayCommand ConnectCommand
        {
            get
            {
                return connectCommand ??
                    (connectCommand = new RelayCommand(async obj =>
                    {
                        FindDeviceInformation();
                        await connection.ConnectAsync(deviceInfromation.Device);
                        //придумать
                        Thread.Sleep(500);
                        showService.ShowMessageBox(connection.GetBluetoothLE().ConnectionStatus.ToString());
                       // messageBoxService.ShowOrOpen(0, messageVM, true);
                       // showService.ShowOrOpen(1, this, false);
                    }));
            }
        }

        private void FindDeviceInformation()
        {
            DeviceInformation device = Devices.Where(x => x.Name == SelectedDevice).FirstOrDefault();
            deviceInfromation.Device = device;     
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
