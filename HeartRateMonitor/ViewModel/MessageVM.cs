using HeartRateMonitor.Model;
using HeartRateMonitor.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.ViewModel
{
    class MessageVM : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private MessageBoxService messageBoxService;
        private WindowService windowService;
        private RelayCommand yesCommand;
        private RelayCommand cancelCommand;
        private MiBand authenticate;
        private ConnectionToBLE connection;
        private OurDeviceInformation device;
        public MessageVM(MessageBoxService messageBoxService, WindowService windowService)
        {
            this.messageBoxService = messageBoxService;
            this.windowService = windowService;
            device = new OurDeviceInformation();
            authenticate = new MiBand();
            connection = ConnectionToBLE.getInstance();
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public RelayCommand YesCommand
        {
            get
            {
                return yesCommand ??
                    (yesCommand = new RelayCommand(async obj =>
                    {
                        await authenticate.AuthenticateAsync(connection.GetBluetoothLE());
                        windowService.ShowOrOpen(1, this, false);
                        messageBoxService.ShowOrOpen(1, this, false);
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
                        connection.Disconnect(device.Device);
                        messageBoxService.ShowOrOpen(1, this, false);
                    }));
            }
        }

    }
}





