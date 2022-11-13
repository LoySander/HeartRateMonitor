using HeartRateMonitor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.ViewModel {
    public class SteamVM: INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _key;
        private string _id;
        private SteamData _steamData;
        private ObservableCollection<Game> _games;
        private Game _selectedGame;
        private ProcessStartInfo _procInfo;
        private Process _process;

        public SteamVM() {
            _steamData = SteamData.getInstance();
            _procInfo = new ProcessStartInfo();
            _procInfo.FileName = @"D:\Steam\steam.exe";
            _process = new Process();
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

        public Game SelectedGame
        {
            get => _selectedGame;
            set
            {
                _selectedGame = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region комманды
        private RelayCommand _enterCommand;
        private RelayCommand _findCommand;
        private RelayCommand _startGameCommand;
        private RelayCommand _exitGameCommand;

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

        public RelayCommand StartGameCommand
        {
            get
            {
                return _startGameCommand ??
                       (_startGameCommand = new RelayCommand(obj =>
                       {
                           if (_selectedGame != null)
                           {
                               _procInfo.Arguments = $"steam://rungameid/{_selectedGame.Appid}";
                               _process.StartInfo = _procInfo;
                               _process.Start();
                           }
                           throw new ArgumentException("Error");
                       }));
            }
        }

        public RelayCommand ExitGameCommand
        {
            get
            {
                return _exitGameCommand ??
                       (_exitGameCommand = new RelayCommand(obj =>
                       {
                           _process.Kill();
                           _process.Close();
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
