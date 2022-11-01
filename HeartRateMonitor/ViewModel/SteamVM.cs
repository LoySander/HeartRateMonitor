using HeartRateMonitor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.ViewModel {
    class SteamVM: INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _key;
        private string _id;
        private SteamData _steamData;

        public SteamVM() {
            _steamData = SteamData.getInstance();
        }

        #region свойства
        public string Key {
            get => _key;
            set {
                _key = value;
                OnPropertyChanged();
            }
        }

        public string ID {
            get => _id;
            set {
                _id = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
