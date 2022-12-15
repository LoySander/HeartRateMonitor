using HeartRateMonitor.Model;
using HeartRateMonitor.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Enumeration;

namespace HeartRateMonitor.ViewModel
{
  
    public class HeartRateVM : INotifyPropertyChanged
    { 
        
        private OurDeviceInformation device;
        private string _selectedDevice;
        private MiBand authenticate;
        private ConnectionToBLE connection;
        private HeartRate heartRate;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isSafeData;
        private bool _isSound;
        private bool _isLearn;
       
        public HeartRateVM()
        {
           device = OurDeviceInformation.getInstance();
           authenticate = new MiBand();
           connection = ConnectionToBLE.getInstance();
           heartRate = HeartRate.getInstance();
        }

        public HeartRate HeartRate
        {
            get { return heartRate; }
            set { heartRate = value;
                OnPropertyChanged(nameof(HeartRate));
            }
        }

        #region command
        private RelayCommand openFindViewCommand;
        private RelayCommand startСommand;
        private RelayCommand disconnectCommand;
        private RelayCommand stopCommand;

        public RelayCommand OpenFindViewCommand
        {
            get
            {
                return openFindViewCommand ??
                    (openFindViewCommand = new RelayCommand(obj =>
                    {
                        //ShowFindView();
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
                        heartRate.StopHeartRate();
                        connection.Disconnect(device.Device);
                        Thread.Sleep(200);
                        //showService.ShowMessageBox(connection.GetBluetoothLE().ConnectionStatus.ToString());
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
                        heartRate.IsSafeData = _isSafeData;
                        heartRate.IsSound = _isSound;
                        heartRate.IsFitting = _isLearn;
                        heartRate.DataPoints = null;
                        if(heartRate.Norm == 0)
                        {
                            heartRate.Norm = 130;
                        }
                        try
                        {
                            await heartRate.StartHeartrateMonitorAsync(connection.GetBluetoothLE());
                        }
                        catch(Exception ex)
                        {
                            await heartRate.StartHeartrateMonitorAsync(connection.GetBluetoothLE());
                        }
                        //SelectedDevice = device.Device.Name.ToString();
                    }));
            }
        }

        public RelayCommand StopCommand
        {
            get
            {
                return stopCommand ??
                    (stopCommand = new RelayCommand(obj =>
                    {
                        heartRate.StopHeartRate();
                        //showService.ShowMessageBox("Stopping");
                    }));
            }
        } 
        #endregion 
       
        public string SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                OnPropertyChanged(nameof(SelectedDevice));
            }
        }
       
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public bool IsSafeData
        {
            get { return _isSafeData; }
            set { _isSafeData = value;
                OnPropertyChanged(nameof(IsSafeData));
               // showService.ShowMessageBox("Хорошо, данные будут записываться в файл");
            }
        }
        
        public bool IsSound
        {
            get { return _isSound; }
            set {
                _isSound = value;
                OnPropertyChanged(nameof(IsSound));
               // showService.ShowMessageBox("Хорошо, вы включили звуковое оповещение");
            }
        }

        public bool IsLearn
        {
            get => _isLearn;
            set
            {
                _isLearn = value;
                OnPropertyChanged();
            }
        }
    }
}
