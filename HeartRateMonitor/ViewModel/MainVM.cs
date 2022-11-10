using HeartRateMonitor.Interfaces;
using HeartRateMonitor.Services;
using HeartRateMonitor.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.ViewModel
{
    public class MainVM: INotifyPropertyChanged
    { 
        private IView _currentPage;
        public event PropertyChangedEventHandler PropertyChanged;

        public List<IView> Pages { get; }

        public IView CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Title => $"Мониторинг ЧСС - {CurrentPage.Title}";

        public MainVM()
        {
            Pages = new List<IView>
            {
                IocContainer.Resolve<ConnectionBLEWindow>(),
                IocContainer.Resolve<HeartRateView>(),
                IocContainer.Resolve<SteamView>(),
                IocContainer.Resolve<InformationUserView>()
            };
            CurrentPage = Pages[0];
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

