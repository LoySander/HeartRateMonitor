using HeartRateMonitor.Interfaces;
using HeartRateMonitor.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HeartRateMonitor.View
{
    /// <summary>
    /// Логика взаимодействия для HeartRateView.xaml
    /// </summary>
    public partial class HeartRateView : Page,IView
    {
        string data = "M2,12L12,3L22,12H19V20H5V12H2M12,18L12.72,17.34C15.3,15 17,13.46 17,11.57C17,10.03 15.79,8.82 14.25,8.82C13.38,8.82 12.55,9.23 12,9.87C11.45,9.23 10.62,8.82 9.75,8.82C8.21,8.82 7,10.03 7,11.57C7,13.46 8.7,15 11.28,17.34L12,18Z";
        public HeartRateView(HeartRateVM heartRateVM)
        {
            InitializeComponent();
            DataContext = heartRateVM;
        }

        public string Data { get => data; set => data = value; }
    }
}
