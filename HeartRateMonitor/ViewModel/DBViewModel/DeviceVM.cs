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
    public class DeviceVM:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<DeviceDTO> Devices { get; set; }

        private DeviceDTO _selectedDevice { get; set; }

        private DeviceModelDB deviceModelDB;

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

        public int Id
        {
            get { return _selectedDevice.Id; }
            set
            {
                _selectedDevice.Id = value;
                OnPropertyChanged();
            }
        }

        public int IdCompany
        {
            get { return _selectedDevice.IdCompany; }
            set
            {
                _selectedDevice.IdCompany = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _selectedDevice.Name; }
            set
            {
                _selectedDevice.Name = value;
                OnPropertyChanged();
            }
        }

        public Byte? Type
        {
            get { return _selectedDevice.Type; }
            set
            {
                _selectedDevice.Type = value;
                OnPropertyChanged();
            }

        }
        
        public string NameCompany
        {
            get { return _selectedDevice.Company.Name; }
            set
            {
                _selectedDevice.Company.Name = value;
                OnPropertyChanged();
            }
        }
      
        #endregion

        public DeviceVM()
        {
            deviceModelDB = new DeviceModelDB();
           // Devices = new ObservableCollection<DeviceDTO>(deviceModelDB.GetAllDevices());
            Devices = new ObservableCollection<DeviceDTO>(deviceModelDB.GetAllDevicesWithCompany());
           
        }

        public void Update()
        {
            using (ApplicationContext context = new ApplicationContext())
            {
                //context.Entry(SelectedBook).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
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
