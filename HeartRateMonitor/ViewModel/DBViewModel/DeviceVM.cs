using HeartRateMonitor.Model.DatabaseModel.Context;
using HeartRateMonitor.Model.DatabaseModel.DTO;
using HeartRateMonitor.Model.DatabaseModel;
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
    public class DeviceVM
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DeviceDTO _DeviceDTO { get; set; }

        private DeviceModelDB _deviceModelDB;

        #region свойства
        public DeviceDTO DeviceDTO
        {
            get { return _DeviceDTO; }
            set
            {
                _DeviceDTO = value;
                OnPropertyChanged();
            }
        }

        //public Company Company
        //{
        //    get { return _selectedDevice.Company; }
        //    set
        //    {
        //        _selectedDevice.Company = value;
        //        OnPropertyChanged();
        //    }
        //}

      
        public string Name
        {
            get { return _DeviceDTO.Name; }
            set
            {
                _DeviceDTO.Name = value;
                OnPropertyChanged();
            }
        }

        public Byte? Type
        {
            get { return _DeviceDTO.Type; }
            set
            {
                _DeviceDTO.Type = value;
                OnPropertyChanged();
            }

        }

        public string NameCompany
        {
            get { return _DeviceDTO.Company.Name; }
            set
            {
                _DeviceDTO.Company.Name = value;
                OnPropertyChanged();
            }
        }

        public Company Company
        {
            get { return _DeviceDTO.Company; }
            set {
                _DeviceDTO.Company = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public DeviceVM(DeviceDTO deviceDTO)
        {
            //deviceModelDB = new DeviceModelDB();
            DeviceDTO = deviceDTO;
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
