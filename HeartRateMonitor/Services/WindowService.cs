using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HeartRateMonitor.Services
{
    class WindowService
    {
        private Window control = null;
       
        public void ShowOrOpen(int p_viewIndex, object p_dataContext, bool p_isModal)
        {
            switch (p_viewIndex)
            {
                case 0:
                    control = new FindWindow();
                    break;
                case 1:
                   
                    break;
                default:
                    throw new ArgumentOutOfRangeException("p_viewIndex", "Такого индекса View не существует");
            }
            if (control != null)
            {
               
                //control.SizeToContent = SizeToContent.WidthAndHeight;
                control.DataContext = p_dataContext;
                if (p_isModal)
                {
                    control.Activate();
                }
                else
                {
                    control.Close();
                }

            }
        }

        public void ShowMessageBox(string str)
        {
            MessageBox.Show(str);
        }
    }
}
