using HeartRateMonitor.Model;
using HeartRateMonitor.Model.DatabaseModel;
using HeartRateMonitor.Model.DatabaseModel.Context;
using HeartRateMonitor.Model.DatabaseModel.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.ViewModel.DBViewModel
{
    public class DevicesVM:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<DeviceDTO> Devices { get; set; }
        public ObservableCollection<Company> Companies { get; set; }

        private DeviceModelDB _deviceModelDB;
        private Company _company;
        private DeviceDTO _selectedDevice { get; set; }
        private string _nameDevice;

       // private DeviceModelDB deviceModelDB;

        #region свойства
        public DeviceDTO SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                OnPropertyChanged();
            }
        }

        public Company Company
        {
            get { return _company; }
            set
            {
                _company = value;
                OnPropertyChanged();
            }
        }

        public string NameDevice
        {
            get { return _nameDevice; }
            set
            {
                _nameDevice = value;
                OnPropertyChanged();
            }
        }


        #endregion

        public DevicesVM()
        {
            _deviceModelDB = new DeviceModelDB();
           // Devices = new ObservableCollection<DeviceVM>(_deviceModelDB.GetAllDeviceDTO());
           Devices = new ObservableCollection<DeviceDTO>(_deviceModelDB.GetAllDevices());
            Companies = new ObservableCollection<Company>(_deviceModelDB.GetAllCompany());
            //Devices = new ObservableCollection<DeviceDTO>(deviceModelDB.GetAllDevicesWithCompany());
           
        }

        public void Update()
        {
            using (ApplicationContext context = new ApplicationContext())
            {
                //context.Entry(SelectedBook).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        #region комманды
        private RelayCommand _AddCommand;
        private RelayCommand _UpdateCommand;
        private RelayCommand _DeleteCommand;
        private RelayCommand _RefreshCommand;
       

        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand ??
                    (_AddCommand = new RelayCommand(obj =>
                    {
                        DeviceDTO deviceDTO = new DeviceDTO() { Name = NameDevice, Company = Company };
                        Devices.Add(deviceDTO);
                        _deviceModelDB.AddNewObject(deviceDTO);
                    }));
            }
        }

        public RelayCommand UpdateCommand
        {
            get
            {
                return _UpdateCommand ??
                    (_UpdateCommand = new RelayCommand(obj =>
                    {
                        _deviceModelDB.UpdateObject(SelectedDevice);
                    }));
            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand ??
                    (_DeleteCommand = new RelayCommand(obj =>
                    {
                        _deviceModelDB.DeleteObject(SelectedDevice);
                    }));
            }
        }

        public RelayCommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ??
                    (_RefreshCommand = new RelayCommand(obj =>
                    {
                        Devices = new ObservableCollection<DeviceDTO>(_deviceModelDB.GetAllDevices());
                        Companies = new ObservableCollection<Company>(_deviceModelDB.GetAllCompany());
                    }));
            }
        }

        #endregion 

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
