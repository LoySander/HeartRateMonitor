using HeartRateMonitor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Game> _games;

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

        public ObservableCollection<Game> Games
        {
            get => _games;
            set
            {
                _games = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region комманды
        private RelayCommand _enterCommand;
        private RelayCommand _findCommand;

        public RelayCommand EnterCommand
        {
            get
            {
                return _enterCommand ??
                    (_enterCommand = new RelayCommand(obj =>
                    {
                        SteamData.Key = Key;
                        SteamData.ID = ID;
                    }));
            }
        }

        public RelayCommand FindCommand
        {
            get 
            {
                return _findCommand ??
                       (_findCommand = new RelayCommand(obj =>
                       {
                           _steamData.GetGames();
                           Games = CollectionEx.ToObservableCollection(_steamData.GamesList);
                       }));
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
