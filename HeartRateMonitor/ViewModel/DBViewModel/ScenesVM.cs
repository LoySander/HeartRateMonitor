using HeartRateMonitor.Model.DatabaseModel;
using HeartRateMonitor.Model.DatabaseModel.Context;
using HeartRateMonitor.Model.DatabaseModel.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.ViewModel.DBViewModel
{
    public class ScenesVM: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public ObservableCollection<SceneDTO> Scenes { get; set; }

        private SceneDTO _selectedScene { get; set; }
        private SceneModelBL _sceneModelBL;

        #region свойства
        public SceneDTO SelectedScene
        {
            get { return _selectedScene; }
            set
            {
                _selectedScene = value;
                OnPropertyChanged();
            }
        }

        public int Id
        {
            get { return _selectedScene.Id; }
            set
            {
                _selectedScene.Id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _selectedScene.Name; }
            set
            {
                _selectedScene.Name = value;
                OnPropertyChanged();
            }
        }

        public int Activity
        {
            get { return _selectedScene.Activity; }
            set
            {
                _selectedScene.Activity = value;
                OnPropertyChanged();
            }
        }

        public int Type
        {
            get { return _selectedScene.Type; }
            set
            {
                _selectedScene.Type = value;
                OnPropertyChanged();
            }

        }

        #endregion

        public ScenesVM()
        {
           _sceneModelBL = new SceneModelBL();
            //Scenes = new ObservableCollection<Scene>(_sceneModelBL.GetAllScenes());
            Scenes = new ObservableCollection<SceneDTO>(_sceneModelBL.GetAllScenesWithType());
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
