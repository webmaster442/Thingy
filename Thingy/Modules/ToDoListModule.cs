using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Thingy.Db;

namespace Thingy.Modules
{
    public class ToDoListModule : IModule
    {
        public string ModuleName
        {
            get { return "To Do List"; }
        }

        public ImageSource Icon
        {
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy.Images;component/Icons/icons8-checklist.png")); }
        }

        public UserControl RunModule()
        {
            var toDoList = new Views.ToDoList
            {
                DataContext = new ViewModels.ToDoListViewModel(App.Instance, App.IoCContainer.ResolveSingleton<IDataBase>())
            };
            return toDoList;
        }
    }
}
