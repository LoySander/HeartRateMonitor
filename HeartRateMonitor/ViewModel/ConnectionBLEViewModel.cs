﻿using HeartRateMonitor.Model;
using HeartRateMonitor.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Windows.Devices.Enumeration;

namespace HeartRateMonitor.ViewModel
{
    class ConnectionBLEViewModel:INotifyPropertyChanged
    {
        private BLE_Device _deviceBLE;
        private ObservableCollection<DeviceInformation> _devices;
        private DeviceInformation _device;
        private ConnectionToBLE _connectionToBLE;
        private MessageBoxService _messageBox;
        public event PropertyChangedEventHandler PropertyChanged;

        public ConnectionBLEViewModel()
        {
            _deviceBLE = BLE_Device.getInstance();
            _connectionToBLE = ConnectionToBLE.getInstance();
            _messageBox = new MessageBoxService();
            Devices = new ObservableCollection<DeviceInformation>();
        }

        #region command
        private RelayCommand _findCommand;
        private RelayCommand _connectCommand;

        public RelayCommand FindCommand
        {
            get
            {
                return _findCommand ??
                    (_findCommand = new RelayCommand(obj =>
                    {
                        _deviceBLE.FindBLE_Device();
                        Devices = CollectionEx.ToObservableCollection(_deviceBLE.Devices);
                        //GenerateBLE_Devices();
                    }));
            }
        }

        public RelayCommand ConnectCommand
        {
            get
            {
                return _connectCommand ??
                    (_connectCommand = new RelayCommand(obj =>
                    {
                        ConnectToBLEDevice();
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

        private async void ConnectToBLEDevice()
        {
            if(Device != null)
            {
               await _connectionToBLE.ConnectAsync(Device);
                _messageBox.ShowOrOpen(0,this,true);
            }
            else
            {
                throw new InvalidOperationException();
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
