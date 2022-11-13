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
        string data = "M14.88,14.29L13,16.17V12.41L14.88,14.29M13,3.83L14.88,5.71L13,7.59M17.71,5.71L12,0H11V7.59L6.41,3L5,4.41L10.59,10L5,15.59L6.41,17L11,12.41V20H12L17.71,14.29L13.41,10L17.71,5.71M15,24H17V22H15M7,24H9V22H7M11,24H13V22H11V24Z";
        public ConnectionBLEWindow(ConnectionBLEViewModel connectionBLEViewModel)
        {
            InitializeComponent();
            DataContext = connectionBLEViewModel;
        }

        public string Data { get => data ; set => data = value; }
    }
}
