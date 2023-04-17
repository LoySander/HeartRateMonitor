using HeartRateMonitor.Services;
using HeartRateMonitor.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HeartRateMonitor.Interfaces;
using HeartRateMonitor.View.DBView;

namespace HeartRateMonitor.ViewModel.DBViewModel
{
    public class DatabaseMainVM : INotifyPropertyChanged
    {
        private IView _currentDatabasePage;
        public event PropertyChangedEventHandler PropertyChanged;

        public List<IView> Databases { get; }

        public IView CurrentDatabasePage
        {
            get => _currentDatabasePage;
            set
            {
                _currentDatabasePage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Title => $"База данных - {CurrentDatabasePage.Title}";

        public DatabaseMainVM()
        {
            Databases = new List<IView>
            {
                IocContainer.Resolve<DevicesView>(),
                IocContainer.Resolve<ParametersView>(),
                IocContainer.Resolve<ScenesView>(),
                IocContainer.Resolve<UsersView>()
            
            };
            CurrentDatabasePage = Databases[0];
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

