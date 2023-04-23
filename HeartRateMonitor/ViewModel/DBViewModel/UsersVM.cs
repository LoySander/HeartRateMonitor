﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HeartRateMonitor.Model.DatabaseModel;
using HeartRateMonitor.Model.DatabaseModel.Context;
using Windows.Media.AppBroadcasting;

namespace HeartRateMonitor.ViewModel.DBViewModel
{
    public class UsersVM: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<User> Users { get; set; }

        private User _selectedUser { get; set; }
        private UserModelBL _userModelBL;

        #region свойства
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        public int Id
        {
            get { return _selectedUser.Id; }
            set 
            {
                _selectedUser.Id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _selectedUser.Name; }  
            set 
            {
                _selectedUser.Name = value;
                OnPropertyChanged();    
            } 
        }

        public string Surname
        {
            get { return _selectedUser.Surname; }
            set 
            { 
                _selectedUser.Surname = value;
                OnPropertyChanged();
            }
        }

        public string MiddleName
        {
            get { return _selectedUser.MiddleName; }
            set 
            { 
                _selectedUser.MiddleName = value; 
                OnPropertyChanged();
            }
           
        }
        public DateTime Birthday
        {
            get { return _selectedUser.Birthday;}
            set
            {
                _selectedUser.Birthday = value;
                OnPropertyChanged();
            }
        }

        public int AverageHeartRate
        {
            get { return _selectedUser.AverageHeartRate; }
            set { 
                _selectedUser.AverageHeartRate = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public UsersVM()
        {
            _userModelBL = new UserModelBL();
            Users = new ObservableCollection<User>(_userModelBL.GetAllUsers());
        }

        public void Update()
        {
            using (ApplicationContext context = new ApplicationContext())
            {
                //context.Entry(SelectedBook).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
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
