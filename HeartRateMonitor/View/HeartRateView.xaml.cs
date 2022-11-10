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
        public HeartRateView(HeartRateVM heartRateVM)
        {
            InitializeComponent();
            DataContext = heartRateVM;
        }
    }
}
