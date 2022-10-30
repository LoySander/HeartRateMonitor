using HeartRateMonitor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.ViewModel
{
    class UserVM: INotifyPropertyChanged
    {
        private string _age;
        private string _heartRateSimple;
        private HeartRate _heartRate;
        public event PropertyChangedEventHandler PropertyChanged;

        public UserVM()
        {
            _heartRate = HeartRate.getInstance();
        }

        #region command
        private RelayCommand setNormHeartRate;

        public RelayCommand SetHeartRateNorm
        {
            get
            {
                return setNormHeartRate ??
                    (setNormHeartRate = new RelayCommand(obj =>
                    {
                        _heartRate.SetNormHeartRate(int.Parse(Age), int.Parse(HeartRateSimple));
                    }));
            }
        }
        #endregion

        public string Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged();
            }
        }

        public string HeartRateSimple
        {
            get => _heartRateSimple;
            set
            {
                _heartRateSimple = value;
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
