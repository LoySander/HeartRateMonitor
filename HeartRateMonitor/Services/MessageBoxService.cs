using HeartRateMonitor.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HeartRateMonitor.Services
{
    class MessageBoxService
    {
        private Window control = null;
        public void ShowOrOpen(int p_viewIndex, object p_dataContext, bool p_isModal)
        {
            switch (p_viewIndex)
            {
                case 0:
                    control = new MyMessageBox();
                    break;
                case 1:

                    break;
                default:
                    throw new ArgumentOutOfRangeException("p_viewIndex", "Такого индекса View не существует");
            }
            if (control != null)
            {
                control.DataContext = p_dataContext;
                if (p_isModal)
                {
                    control.Show();
                }
                else
                {
                    control.Close();
                }
            }
        }
    }
}