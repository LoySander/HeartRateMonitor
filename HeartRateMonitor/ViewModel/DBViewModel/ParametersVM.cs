using HeartRateMonitor.Model.DatabaseModel;
using HeartRateMonitor.Model.DatabaseModel.Context;
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
    public class ParametersVM
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Physiological_parameter> Parameters { get; set; }

        private Physiological_parameter _selectedParameter { get; set; }
        private ParameterModelBL _parameterModelBL;

        #region свойства
        public Physiological_parameter SelectedParameter
        {
            get { return _selectedParameter; }
            set
            {
                _selectedParameter = value;
                OnPropertyChanged();
            }
        }

        public int Id
        {
            get { return _selectedParameter.Id; }
            set
            {
                _selectedParameter.Id = value;
                OnPropertyChanged();
            }
        }

        public string HeartRate
        {
            get { return _selectedParameter.HeartRate; }
            set
            {
                _selectedParameter.HeartRate = value;
                OnPropertyChanged();
            }
        }

        public string Pressure
        {
            get { return _selectedParameter.Pressure; }
            set
            {
                _selectedParameter.Pressure = value;
                OnPropertyChanged();
            }
        }

        public string Temperature
        {
            get { return _selectedParameter.Temperature; }
            set
            {
                _selectedParameter.Temperature = value;
                OnPropertyChanged();
            }

        }

        public string Saturation
        {
            get { return _selectedParameter.Saturation; }
            set
            {
                _selectedParameter.Saturation = value;
                OnPropertyChanged();
            }

        }


        #endregion

        public ParametersVM()
        {
            _parameterModelBL = new ParameterModelBL();
            Parameters = new ObservableCollection<Physiological_parameter>(_parameterModelBL.GetAllParameters());
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
