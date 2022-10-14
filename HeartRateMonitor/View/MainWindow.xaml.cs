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

namespace HeartRateMonitor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM();
        }
    
        private void MessageButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Привет");
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            FindWindow z = new FindWindow();
            z.Activate();
        }

        private void sideBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = sideBar.SelectedItem as NavButton;
            navFrame.Navigate(selected.Navlink);
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            var selected = sender as NavButton;
            navFrame.Navigate(selected.Navlink);
        }
    }
}
