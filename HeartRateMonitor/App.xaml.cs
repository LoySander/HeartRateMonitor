using HeartRateMonitor.Services;
using HeartRateMonitor.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HeartRateMonitor
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IocContainer.Register<HeartRateVM>();
            IocContainer.Register<ConnectionBLEViewModel>();
            IocContainer.Register<SteamVM>();
            IocContainer.Register<UserVM>();
            IocContainer.Register<MainVM>();
            IocContainer.Register<DatabaseVM>();

            IocContainer.Resolve<MainWindow>().Show();
        }

    }
}
