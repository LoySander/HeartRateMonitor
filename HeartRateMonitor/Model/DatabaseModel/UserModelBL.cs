using HeartRateMonitor.Model.DatabaseModel.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model.DatabaseModel
{
    public class UserModelBL
    {
        public IEnumerable<User> GetAllUsers()
        {
            using (ApplicationContext context = new ApplicationContext())
            {
                /// Надо ОБЯЗАТЕЛЬНО избавиться от DataSet
                /// Выше по уровне он не должен подымать ни в коем случае.
                return context.Users.ToArray();
            }
        }
    }
}
