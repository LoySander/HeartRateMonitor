using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HeartRateMonitor.Interfaces;
using HeartRateMonitor.ViewModel;

namespace HeartRateMonitor.View
{
    /// <summary>
    /// Логика взаимодействия для ConnectionBLEWindow.xaml
    /// </summary>
    public partial class ConnectionBLEWindow : Page, IView
    {
        public ConnectionBLEWindow(ConnectionBLEViewModel connectionBLEViewModel)
        {
            InitializeComponent();
            DataContext = connectionBLEViewModel;
        }
    }
}
